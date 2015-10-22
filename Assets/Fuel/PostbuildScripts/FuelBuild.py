#!/usr/bin/python

import sys
import shutil
import os
import fileinput

from mod_pbxproj import XcodeProject

def exitWithError( message ):
	print 'Error: ' + message
	sys.exit('Failed automatic integration')

if len(sys.argv) < 6:
	exitWithError('Argument list does not contain enough arguments')

pluginPath = sys.argv[1]
xcodePath = sys.argv[2]
projectPath = sys.argv[3]
dataPath = sys.argv[4]
unityApiLevel = int(sys.argv[5].strip())
xcodeVersion = sys.argv[6]

# Unity API levels
# 0 - UNSUPPORTED
# 1 - UNITY_2_6,
# 2 - UNITY_2_6_1,
# 3 - UNITY_3_0,
# 4 - UNITY_3_0_0,
# 5 - UNITY_3_1,
# 6 - UNITY_3_2,
# 7 - UNITY_3_3,
# 8 - UNITY_3_4,
# 9 - UNITY_3_5,
# 10 - UNITY_4_0,
# 11 - UNITY_4_0_1,
# 12 - UNITY_4_1,
# 13 - UNITY_4_2,
# 14 - UNITY_4_3,
# 15 - UNITY_4_5,
# 16 - UNITY_4_6,
# 17 - UNITY_5_0,
# 18 - UNITY_5_1

# only supporting Unity 3.5 and up
if unityApiLevel < 9:
	exitWithError('Unsupported Unity version ' + `unityApiLevel`)

def checkPath(path, libname):
	if path == None:
		exitWithError('Unable to find ' + libname + ' which is required for Propeller integration. Add it manually or modify FuelBuild.py to find the correct folder.')

print 'Adding Propeller dependencies to project'

systemConfigurationFrameworkPath = None
adSupportFrameworkPath = None
accountsFrameworkPath = None
socialFrameworkPath = None
securityFrameworkPath = None
cfNetworkFrameworkPath = None
audioToolboxFrameworkPath = None
webKitFrameworkPath = None
javascriptFramework = None
libsqlite3Path = None
libicucorePath = None

for directory, dirnames, filenames in os.walk( xcodePath + '/Platforms/iPhoneOS.platform/Developer/SDKs' ):
	if os.path.basename( directory ) == 'SystemConfiguration.framework':
		systemConfigurationFrameworkPath = directory
	elif os.path.basename( directory ) == 'AdSupport.framework':
		adSupportFrameworkPath = directory
	elif os.path.basename( directory ) == 'Social.framework':
		socialFrameworkPath = directory
	elif os.path.basename( directory ) == 'Security.framework':
		securityFrameworkPath = directory
	elif os.path.basename( directory ) == 'CFNetwork.framework':
		cfNetworkFrameworkPath = directory
	elif os.path.basename( directory ) == 'AudioToolbox.framework':
		audioToolboxFrameworkPath = directory
	elif os.path.basename( directory ) == 'WebKit.framework':
		webKitFrameworkPath = directory
	elif os.path.basename( directory ) == 'JavaScriptCore.framework':
		javascriptFramework = directory
	else:
		if 'libsqlite3.tbd' in filenames:
			libsqlite3Path = directory + '/libsqlite3.tbd'
		if 'libicucore.tbd' in filenames:
			libicucorePath = directory + '/libicucore.tbd'

print 'Locating frameworks and resources required by Propeller'

checkPath( systemConfigurationFrameworkPath, 'SystemConfiguration.framework' )
checkPath( adSupportFrameworkPath, 'AdSupport.framework' )
checkPath( socialFrameworkPath, 'Social.framework' )
checkPath( securityFrameworkPath, 'Security.framework' )
checkPath( cfNetworkFrameworkPath, 'CFNetwork.framework' )
checkPath( audioToolboxFrameworkPath, 'AudioToolbox.framework' )
checkPath( webKitFrameworkPath, 'WebKit.framework' )
checkPath( javascriptFramework, 'JavaScriptCore.framework' )
checkPath( libsqlite3Path, 'libsqlite3.tbd' )
checkPath( libicucorePath, 'libicucore.tbd' )

project = XcodeProject.Load( projectPath + '/Unity-iPhone.xcodeproj/project.pbxproj' )

print 'Adding frameworks required by Propeller'

frameworkGroup = project.get_or_create_group('Frameworks')

project.add_file_if_doesnt_exist( systemConfigurationFrameworkPath, tree='SDKROOT', parent=frameworkGroup )
project.add_file_if_doesnt_exist( adSupportFrameworkPath, tree='SDKROOT', parent=frameworkGroup )
project.add_file_if_doesnt_exist( socialFrameworkPath, tree='SDKROOT', parent=frameworkGroup )
project.add_file_if_doesnt_exist( securityFrameworkPath, tree='SDKROOT', parent=frameworkGroup )
project.add_file_if_doesnt_exist( cfNetworkFrameworkPath, tree='SDKROOT', parent=frameworkGroup )
project.add_file_if_doesnt_exist( audioToolboxFrameworkPath, tree='SDKROOT', parent=frameworkGroup )
project.add_file_if_doesnt_exist( webKitFrameworkPath, tree='SDKROOT', parent=frameworkGroup )
project.add_file_if_doesnt_exist( javascriptFramework, tree='SDKROOT', parent=frameworkGroup )
project.add_file_if_doesnt_exist( libsqlite3Path, tree='SDKROOT', parent=frameworkGroup )
project.add_file_if_doesnt_exist( libicucorePath, tree='SDKROOT', parent=frameworkGroup )

print 'Inserting Propeller libraries'

classesGroup = project.get_or_create_group( 'Classes' )
librariesGroup = project.get_or_create_group( 'Libraries' )

if unityApiLevel < 17:
	shutil.copy( pluginPath + '/PropellerSDK.h' , projectPath + '/Classes/PropellerSDK.h' )
	shutil.copy( pluginPath + '/libPropellerSDK.a', projectPath + '/Libraries/libPropellerSDK.a' )
	shutil.copy( pluginPath + '/FuelSDKUnityWrapper.mm', projectPath + '/Libraries/FuelSDKUnityWrapper.mm' )
	project.add_file_if_doesnt_exist( projectPath + '/Classes/PropellerSDK.h', parent=classesGroup )
	project.add_file_if_doesnt_exist( projectPath + '/Libraries/libPropellerSDK.a', parent=librariesGroup )
	project.add_file_if_doesnt_exist( projectPath + '/Libraries/FuelSDKUnityWrapper.mm', parent=librariesGroup )

project.saveFormat3_2()

# Unity 4.2 changed the name of the AppController.mm file to UnityAppController.mm
controllerFilename = ''

if unityApiLevel < 13:
	controllerFilename = 'AppController.mm'
else:
	controllerFilename = 'UnityAppController.mm'

controllerFile = projectPath + '/Classes/' + controllerFilename

print 'Injecting Propeller script into ' + controllerFilename

# inject code into AppController.mm or UnityAppController.m

injectionPrefix = '// *** INSERTED BY PROPELLER BUILD SCRIPT ***'
injectionSuffix = '// *** END PROPELLER BUILD SCRIPT INSERTION ***'

def addInjectionPrefix(indents=0):
	print '\t' * indents + injectionPrefix
	print ''

def addInjectionSuffix(indents=0):
	print ''
	print '\t' * indents + injectionSuffix
	print ''

def addFunctionInjectionPrefix(signature, contentOnly=False, contentIndents=1):
	if not contentOnly:
		addInjectionPrefix()
		print signature
		print '{'
	else:
		addInjectionPrefix(contentIndents)

def addFunctionInjectionSuffix(contentOnly=False, contentIndents=1):
	if not contentOnly:
		print '}'
		addInjectionSuffix()
	else:
		addInjectionSuffix(contentIndents)

def addHeader():
	addInjectionPrefix()
	print '#include "fuel.h"'
	print '#include "fuelcompete.h"'
	print '#include "fuelcompeteui.h"'
	print '#include "fueldynamics.h"'
	print '#include "fuelignite.h"'
	print '#include "fueligniteui.h"'
	print '#include "fueltypes.h"'
	print ''
	print '#define encodeString(rawString) CFBridgingRelease(CFURLCreateStringByAddingPercentEscapes(NULL, (CFStringRef)rawString, NULL, (CFStringRef)@"!*\'();:@&=+$,/?%#[]",kCFStringEncodingUTF8))'

	addInjectionSuffix()

def addInit(contentOnly=False):
	addFunctionInjectionPrefix('- (id)init', contentOnly, 2)
	if not contentOnly:
		print '\tif( (self = [super init]) )'
		print '\t{'
	print '\t\t[[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(receiveLocalNotification:) name:FSDK_BROADCAST_COMPETE_CHALLENGE_COUNT object:nil];'
	print '\t\t[[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(receiveLocalNotification:) name:FSDK_BROADCAST_COMPETE_TOURNAMENT_INFO object:nil];'
	print '\t\t[[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(receiveLocalNotification:) name:FSDK_BROADCAST_VG_LIST object:nil];'
	print '\t\t[[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(receiveLocalNotification:) name:FSDK_BROADCAST_VG_ROLLBACK object:nil];'
	print '\t\t[[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(receiveLocalNotification:) name:FSDK_BROADCAST_USER_VALUES object:nil];'
	print '\t\t[[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(receiveLocalNotification:) name:FSDK_BROADCAST_SOCIAL_LOGIN_REQUEST object:nil];'
	print '\t\t[[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(receiveLocalNotification:) name:FSDK_BROADCAST_SOCIAL_INVITE_REQUEST object:nil];'
	print '\t\t[[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(receiveLocalNotification:) name:FSDK_BROADCAST_SOCIAL_SHARE_REQUEST object:nil];'
	print '\t\t[[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(receiveLocalNotification:) name:FSDK_BROADCAST_COMPETE_EXIT object:nil];'
	print '\t\t[[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(receiveLocalNotification:) name:FSDK_BROADCAST_COMPETE_MATCH object:nil];'
	print '\t\t[[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(receiveLocalNotification:) name:FSDK_BROADCAST_COMPETE_FAIL object:nil];'
	print '\t\t[[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(receiveLocalNotification:) name:FSDK_BROADCAST_IGNITE_EVENTS object:nil];'
	print '\t\t[[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(receiveLocalNotification:) name:FSDK_BROADCAST_IGNITE_LEADERBOARD object:nil];'
	print '\t\t[[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(receiveLocalNotification:) name:FSDK_BROADCAST_IGNITE_MISSION object:nil];'
	print '\t\t[[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(receiveLocalNotification:) name:FSDK_BROADCAST_IGNITE_QUEST object:nil];'
	print '\t\t[[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(receiveLocalNotification:) name:FSDK_BROADCAST_IMPLICIT_LAUNCH_REQUEST object:nil];'

	if not contentOnly:
		print '\t}'
		print '\treturn self;'
	addFunctionInjectionSuffix(contentOnly, 2)

def addDealloc(contentOnly=False):
	addFunctionInjectionPrefix('- (void) dealloc', contentOnly)
	print '\t[[NSNotificationCenter defaultCenter] removeObserver:self];'
	if not contentOnly:
		print '#if !__has_feature(objc_arc)'
		print '\t[super dealloc];'
		print '#endif'
	addFunctionInjectionSuffix(contentOnly)

def addReceiveLocalNotification(contentOnly=False):
	addFunctionInjectionPrefix('- (void)application:(UIApplication*)application didReceiveLocalNotification:(UILocalNotification*)notification', contentOnly)
	print '\t[fuel handleLocalNotification:notification newLaunch:NO];'
	addFunctionInjectionSuffix(contentOnly)

def addRegisterRemoteNotifications(contentOnly=False):
	addFunctionInjectionPrefix('- (void)application:(UIApplication*)application didRegisterForRemoteNotificationsWithDeviceToken:(NSData*)deviceToken', contentOnly)
	print '\tNSString *devToken = [[deviceToken description] stringByTrimmingCharactersInSet:[NSCharacterSet characterSetWithCharactersInString:@"<>"]];'
	print '\tdevToken = [devToken stringByReplacingOccurrencesOfString:@" " withString:@""];'
	print '\t[fuel setNotificationToken:devToken];'
	addFunctionInjectionSuffix(contentOnly)

def addFailToRegisterRemoteNotifications(contentOnly=False):
	addFunctionInjectionPrefix('- (void)application:(UIApplication*)application didFailToRegisterForRemoteNotificationsWithError:(NSError*)error', contentOnly)
	print '\t[fuel setNotificationToken:nil];'
	addFunctionInjectionSuffix(contentOnly)

def addReceiveRemoteNotification(contentOnly=False):
	addFunctionInjectionPrefix('- (void)application:(UIApplication*)application didReceiveRemoteNotification:(NSDictionary*)userInfo', contentOnly)
	print '\t[fuel handleRemoteNotification:userInfo newLaunch:NO];'
	addFunctionInjectionSuffix(contentOnly)

def addRegisterUserNotificationSettings(contentOnly=False):
	print '#ifdef __IPHONE_8_0'
	print ''
	addFunctionInjectionPrefix('- (void)application:(UIApplication *)application didRegisterUserNotificationSettings:(UIUserNotificationSettings *)notificationSettings', contentOnly)
	print '\t[application registerForRemoteNotifications];'
	addFunctionInjectionSuffix(contentOnly)
	print '#endif'
	print ''

def addFinishLaunching(contentOnly=False):
	addFunctionInjectionPrefix('- (BOOL)application:(UIApplication*)application didFinishLaunchingWithOptions:(NSDictionary*)launchOptions', contentOnly)
	print '\tNSDictionary *remoteNotificationDict = [launchOptions objectForKey:UIApplicationLaunchOptionsRemoteNotificationKey];'
	print ''
	print '\tif (remoteNotificationDict)'
	print '\t{'
	print '\t\tif (![fuel handleRemoteNotification:remoteNotificationDict newLaunch:YES]) {'
	print '\t\t\t// This is not a Fuel remote notification, I should handle it'
	print '\t\t}'
	print '\t}'
	print ''
	print '\tUILocalNotification *localNotification = [launchOptions objectForKey:UIApplicationLaunchOptionsLocalNotificationKey];'
	print ''
	print '\tif (localNotification)'
	print '\t{'
	print '\t\tif (![fuel handleLocalNotification:localNotification newLaunch:YES]) {'
	print '\t\t\t// This is not a Fuel local notification, I should handle it'
	print '\t\t}'
	print '\t}'
	print ''
	print '\t// we want to register this device with the APNS'
	print '#if __IPHONE_OS_VERSION_MAX_ALLOWED >= 80000'
	print '\tif ([[[UIDevice currentDevice] systemVersion] compare:@"8.0" options:NSNumericSearch] != NSOrderedAscending) {'
	print '\t\tUIUserNotificationType userNotificationTypes = (UIUserNotificationTypeAlert |'
	print '                                                        UIUserNotificationTypeBadge |'
	print '                                                        UIUserNotificationTypeSound);'
	print '\t\tUIUserNotificationSettings *userNotificationSettings = [UIUserNotificationSettings'
	print '                                                                settingsForTypes:userNotificationTypes'
	print '                                                                      categories:nil];'
	print '\t\t[[UIApplication sharedApplication] registerUserNotificationSettings:userNotificationSettings];'
	print '\t} else {'
	print '\t\tUIRemoteNotificationType remoteNotificationTypes = (UIRemoteNotificationTypeAlert |'
	print '                                                            UIRemoteNotificationTypeBadge |'
	print '                                                            UIRemoteNotificationTypeSound);'
	print '\t\t[[UIApplication sharedApplication] registerForRemoteNotificationTypes:remoteNotificationTypes];'
	print '\t}'
	print '#else'
	print '\tUIRemoteNotificationType remoteNotificationTypes = (UIRemoteNotificationTypeAlert |'
	print '                                                        UIRemoteNotificationTypeBadge |'
	print '                                                        UIRemoteNotificationTypeSound);'
	print '\t[[UIApplication sharedApplication] registerForRemoteNotificationTypes:remoteNotificationTypes];'
	print '#endif'
	addFunctionInjectionSuffix(contentOnly)

def addEnterBackground(contentOnly=False):
	addFunctionInjectionPrefix('- (void)applicationDidEnterBackground:(UIApplication*)application', contentOnly)
	print '\t[[UIApplication sharedApplication] setApplicationIconBadgeNumber:1];'
	print '\t[[UIApplication sharedApplication] setApplicationIconBadgeNumber:0];'
	print '\t[[UIApplication sharedApplication] cancelAllLocalNotifications];'
	print '\t[fuel restoreAllLocalNotifications];'
	addFunctionInjectionSuffix(contentOnly)

def addExtraFunctions():
	addInjectionPrefix()
	print '-(NSString *)urlEncode:(NSString *)rawString'
	print '{'
	print '\treturn CFBridgingRelease(CFURLCreateStringByAddingPercentEscapes(NULL, (CFStringRef)rawString, NULL, (CFStringRef)@"!*\'();:@&=+$,/?%#[]", kCFStringEncodingUTF8));'
	print '}'	
	print ''
	print '-(void)receiveLocalNotification:(NSNotification *) notification'
	print '{'
	print '\tNSDictionary *data = notification.userInfo;'
	print '\tNSString *type = [notification name];'
	print ''
	print '\tNSString *message = nil;'
	print '\tif (data) {'
	print '\t\tNSError *error = nil;'
	print '\t\tNSData* jsonData = [NSJSONSerialization dataWithJSONObject:data options:kNilOptions error:&error];'
	print '\t\tNSString *paramsString = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];'
	print ''
	print '\t\tmessage = [NSString stringWithFormat:@"{'+'\\'+'"action'+'\\'+'": '+'\\'+'"%s'+'\\'+'" , '+'\\'+'"data'+'\\'+'" : %s}", [type UTF8String], [paramsString UTF8String]];'
	print '\t} else {'
	print '\t\tmessage = [NSString stringWithFormat:@"{'+'\\'+'"action'+'\\'+'": '+'\\'+'"%s'+'\\'+'" , '+'\\'+'"data'+'\\'+'" : '+'\\'+'"'+'\\'+'"}", [type UTF8String]];'
	print '\t}'
	print '\tUnitySendMessage("FuelSDK", "DataReceiver", [message UTF8String]);'
	print '}'
	addInjectionSuffix()

injectHeader = True
injectInit = False
injectReceiveLocalNotification = False
injectRegisterRemoteNotifications = False
injectFailToRegisterRemoteNotifications = False
injectReceiveRemoteNotification = False
injectRegisterUserNotificationSettings = False
injectFinishLaunching = False
injectEnterBackground = False

injectedHeader = False
injectedInit = False
injectedDealloc = False
injectedReceiveLocalNotification = False
injectedRegisterRemoteNotifications = False
injectedFailToRegisterRemoteNotifications = False
injectedReceiveRemoteNotification = False
injectedRegisterUserNotificationSettings = False
injectedFinishLaunching = False
injectedEnterBackground = False

lastNonBlankLine = ''

for line in fileinput.input( controllerFile, inplace=1 ):
	if len(line.strip()) == 0:
		print line,
		continue

	if '[super init]' in line:
		injectInit = True
	elif '[super dealloc]' in line:
		if injectionSuffix not in lastNonBlankLine:
			addDealloc(True)
			injectedDealloc = True
	elif 'didReceiveLocalNotification:(' in line:
		injectReceiveLocalNotification = True
	elif 'didRegisterForRemoteNotificationsWithDeviceToken:(' in line:
		injectRegisterRemoteNotifications = True
	elif 'didFailToRegisterForRemoteNotificationsWithError:(' in line:
		injectFailToRegisterRemoteNotifications = True
	elif 'didReceiveRemoteNotification:(' in line:
		injectReceiveRemoteNotification = True
	elif 'didRegisterUserNotificationSettings:(' in line:
		injectRegisterUserNotificationSettings = True
	elif 'didFinishLaunchingWithOptions:(' in line:
		injectFinishLaunching = True
	elif ')applicationDidEnterBackground:(' in line:
		injectEnterBackground = True
	else:
		if injectHeader:
			injectHeader = False
			if injectionPrefix not in line:
				addHeader()
				injectedHeader = True
		if injectInit and '{' not in line:
			injectInit = False
			if injectionPrefix not in line:
				addInit(True)
				injectedInit = True
		if injectReceiveLocalNotification and '{' not in line:
			injectReceiveLocalNotification = False
			if injectionPrefix not in line:
				addReceiveLocalNotification(True)
				injectedReceiveLocalNotification = True
		if injectRegisterRemoteNotifications and '{' not in line:
			injectRegisterRemoteNotifications = False
			if injectionPrefix not in line:
				addRegisterRemoteNotifications(True)
				injectedRegisterRemoteNotifications = True
		if injectFailToRegisterRemoteNotifications and '{' not in line:
			injectFailToRegisterRemoteNotifications = False
			if injectionPrefix not in line:
				addFailToRegisterRemoteNotifications(True)
				injectedFailToRegisterRemoteNotifications = True
		if injectReceiveRemoteNotification and '{' not in line:
			injectReceiveRemoteNotification = False
			if injectionPrefix not in line:
				addReceiveRemoteNotification(True)
				injectedReceiveRemoteNotification = True
		if injectRegisterUserNotificationSettings and '{' not in line:
			injectRegisterUserNotificationSettings = False
			if injectionPrefix not in line:
				addRegisterUserNotificationSettings(True)
				injectedRegisterUserNotificationSettings = True
		if injectFinishLaunching and '{' not in line:
			injectFinishLaunching = False;
			if injectionPrefix not in line:
				addFinishLaunching(True)
				injectedFinishLaunching = True
		if injectEnterBackground and '{' not in line:
			injectEnterBackground = False
			if injectionPrefix not in line:
				addEnterBackground(True)
				injectedEnterBackground = True

	lastNonBlankLine = line

	print line,

fileinput.close()

controllerFilenameIndex = controllerFilename.find('.')
controllerFilenameString = controllerFilename[0:controllerFilenameIndex]
controllerFileImplementation = '@implementation ' + controllerFilenameString

injectExtraFunctions = False

lastNonBlankLine = ''

for line in fileinput.input( controllerFile, inplace=1 ):
	if len(line.strip()) == 0:
		print line,
		continue

	if controllerFileImplementation in line:
		injectExtraFunctions = True
	else:
		if injectExtraFunctions and '@end' in line:
			injectExtraFunctions = False
			if injectionSuffix not in lastNonBlankLine:
				if not injectedInit:
					addInit()
				if not injectedDealloc:
					addDealloc()
				if not injectedReceiveLocalNotification:
					addReceiveLocalNotification()
				if not injectedRegisterRemoteNotifications:
					addRegisterRemoteNotifications()
				if not injectedFailToRegisterRemoteNotifications:
					addFailToRegisterRemoteNotifications()
				if not injectedReceiveRemoteNotification:
					addReceiveRemoteNotification()
				if not injectedRegisterUserNotificationSettings:
					addRegisterUserNotificationSettings()
				if not injectedFinishLaunching:
					addFinishLaunching()
				if not injectedEnterBackground:
					addEnterBackground()
				addExtraFunctions()

	lastNonBlankLine = line

	print line,

fileinput.close()

def getFilePath(fileName):
	files = project.get_files_by_name(fileName)

	if len(files) != 1:
		exitWithError('Unable to locate the PBX file reference for ' + fileName)

	filePath = files[0].get('path')

	if filePath == None:
		exitWithError('PBX file reference for ' + fileName + ' is missing a file path')

	return projectPath + '/' + filePath

def addGLViewControllerExport():
	functionExport = True

	for line in fileinput.input( projectPath + '/Classes/AppController.h', inplace=1 ):
		if functionExport:
			if injectionPrefix not in line:
				addInjectionPrefix()
				print 'UIViewController* UnityGetGLViewController();'
				addInjectionSuffix()
			functionExport = False

		print line,

	fileinput.close()

def addWrapperHeader(headerFile):
	headerCorrect = False
	injectHeader = False

	for line in fileinput.input( getFilePath('FuelSDKUnityWrapper.mm'), inplace=1 ):
		if not headerCorrect:
			if injectHeader:
				if '#import "' + headerFile + '"' not in line:
					print '#import "' + headerFile + '"'
				headerCorrect = True
			else:
				if '#import "PropellerSDK.h"' in line:
					injectHeader = True

		print line,

	fileinput.close()

def addImport(sourceFile, headerFile):
	headerCorrect = False

	for line in fileinput.input( projectPath + '/' + sourceFile, inplace=1 ):
		if not headerCorrect:
			if injectionPrefix not in line:
				addInjectionPrefix()
				print '#import ' + headerFile
				addInjectionSuffix()
			headerCorrect = True

		print line,

	fileinput.close()

print 'Injecting compatibility shim into FuelSDKUnityWrapper.mm'

if unityApiLevel >= 15:
	addWrapperHeader('UI/UnityViewControllerBase.h')
elif unityApiLevel >= 10:
	addWrapperHeader('iPhone_View.h')
else:
	addWrapperHeader('AppController.h')

print 'Injecting additional compatibility shims'

if (unityApiLevel == 13) or (unityApiLevel == 14):
	xcodeMajorVersionIndex = xcodeVersion.find('.')
	xcodeMajorVersionString = xcodeVersion[0:xcodeMajorVersionIndex]
	xcodeMajorVersion = int(xcodeMajorVersionString)

	if xcodeMajorVersion >= 6:
		addImport('Classes/Unity/CMVideoSampling.mm', '<OpenGLES/ES2/glext.h>')

if unityApiLevel < 10:
	addGLViewControllerExport()

print 'Propeller SDK code injection completed!'

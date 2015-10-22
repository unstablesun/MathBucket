#import "fuel.h"
#import "fuelcompete.h"
#import "fuelcompeteui.h"
#import "fueldynamics.h"
#import "fuelignite.h"
#import "fueligniteui.h"
#import "fueltypes.h"

extern "C"
{
#define encodeString(rawString) CFBridgingRelease(CFURLCreateStringByAddingPercentEscapes(NULL, (CFStringRef)rawString, NULL, (CFStringRef)@"!*'();:@&=+$,/?%#[]",kCFStringEncodingUTF8))
    
    //TODO
    //static PropellerUnityListener* gsPropellerUnityListener;
    static BOOL autoRotate;
    
    static void validateOrientation();
    
#define NORMALIZED_JSON_DATATYPE_INT	0
#define NORMALIZED_JSON_DATATYPE_LONG	1
#define NORMALIZED_JSON_DATATYPE_FLOAT	2
#define NORMALIZED_JSON_DATATYPE_DOUBLE	3
#define NORMALIZED_JSON_DATATYPE_BOOL	4
#define NORMALIZED_JSON_DATATYPE_STRING	5
    
    static NSObject* normalizeJSONDictionary(NSDictionary *dictionary);
    static NSObject* normalizeJSONList(NSArray *array);
    static NSObject* normalizeJSONValue(NSDictionary *valueDictionary);
    static BOOL isNormalizedJSONValue(NSDictionary *dictionary);
    static NSArray* normalizeList(NSString *valueString);
    
    void iOSInitialize(const char* key, const char* secret, bool hasLogin, bool hasInvite, bool hasShare)
    {
        
        [fuel setup:[NSString stringWithUTF8String:key] gameSecret:[NSString stringWithUTF8String:secret] gameHasLogin:(BOOL)hasLogin gameHasInvite:(BOOL)hasInvite gameHasShare:(BOOL)hasShare];
        
    }
    
    void iOSInitializeDynamics()
    {
        [fueldynamics setup];
    }
    
    
    void iOSSetLanguageCode(const char* languageCode)
    {
        [[fuel instance] setLanguageCode:[NSString stringWithUTF8String:languageCode]];
    }
    
    BOOL iOSLaunch()
    {
        validateOrientation();
        return [[fuelcompeteui instance] launch];
    }
    
    BOOL iOSSubmitMatchResult(const char* data)
    {
        //TODO check the data type for this.
        NSError *error = nil;
        NSString* matchResultString = [NSString stringWithFormat:@"%s" , data];
        NSData* matchResultData = [matchResultString dataUsingEncoding:NSUTF8StringEncoding];
        NSDictionary *matchResult = [NSJSONSerialization JSONObjectWithData:matchResultData
                                                                    options:NSJSONReadingMutableContainers
                                                                      error:&error];
        
        if (error != nil) {
            return false;
        }
        
        matchResult = (NSDictionary*)normalizeJSONDictionary(matchResult);
        
        if (matchResult == nil) {
            return false;
        }
        
        fuelcompete *gSDK = [fuelcompete instance];
        
        return   [gSDK submitMatchResult:matchResult];
    }
    
    void iOSSyncChallengeCounts()
    {
        [[fuelcompete instance] syncChallengeCounts];
    }
    
    void iOSSyncTournamentInfo()
    {
        [[fuelcompete instance] syncTournamentInfo];
    }
    
    void iOSSyncVirtualGoods()
    {
        [[fuel instance] syncVirtualGoods];
    }
    
    void iOSAcknowledgeVirtualGoods(const char* transactionId, BOOL consumed)
    {
        NSString* transactionIdString = nil;
        
        if (transactionId) {
            transactionIdString = [NSString stringWithFormat:@"%s", transactionId];
        }
        
        [[fuel instance] acknowledgeVirtualGoods:transactionIdString consumed:consumed];
    }
    
    BOOL iOSEnableNotification(fuelnotificationtype notificationType)
    {
        return [fuel enableNotification:notificationType];
    }
    
    BOOL iOSDisableNotification(fuelnotificationtype notificationType)
    {
        return [fuel disableNotification:notificationType];
    }
    
    BOOL iOSIsNotificationEnabled(fuelnotificationtype notificationType)
    {
        return [fuel  isNotificationEnabled:notificationType];
    }
    
    BOOL iOSSdkSocialLoginCompleted(const char* urlEncodedCString)
    {
        NSMutableDictionary *loginInfo = nil;
        
        if (urlEncodedCString != nil) {
            loginInfo = [[NSMutableDictionary alloc] init];
            
            NSString* urlEncodedString = [NSString stringWithFormat:@"%s" , urlEncodedCString];
            
            NSArray* keyValuePairs = [urlEncodedString componentsSeparatedByString: @"&"];
            
            for (NSString* keyValuePairString in keyValuePairs) {
                NSArray* keyValuePair = [keyValuePairString componentsSeparatedByString: @"="];
                
                NSString* key = [keyValuePair[0] stringByReplacingPercentEscapesUsingEncoding:NSUTF8StringEncoding];
                NSString* value = [keyValuePair[1] stringByReplacingPercentEscapesUsingEncoding:NSUTF8StringEncoding];
                
                [loginInfo setObject:value forKey:key];
            }
        }
        
        BOOL result = [[fuel instance] sdkSocialLoginCompleted:loginInfo];
        
        if (loginInfo != nil) {
#if !__has_feature(objc_arc)
            [loginInfo release];
#endif
            loginInfo = nil;
        }
        
        return result;
    }
    
    BOOL iOSSdkSocialInviteCompleted()
    {
        return [[fuel instance] sdkSocialInviteCompleted];
    }
    
    BOOL iOSSdkSocialShareCompleted()
    {
        return [[fuel instance] sdkSocialShareCompleted];
    }
    
    void iOSRestoreAllLocalNotifications()
    {
        [fuel restoreAllLocalNotifications];
    }
    
    void validateOrientation()
    {
        if (!autoRotate) {
            return;
        }
        
        UIInterfaceOrientation orientation = [[UIApplication sharedApplication] statusBarOrientation];
        
        if (UIInterfaceOrientationIsLandscape(orientation)) {
            [[fuelcompeteui instance] setOrientation:fuelgameorientationlandscape];
        } else if (UIInterfaceOrientationIsPortrait(orientation)) {
            [[fuelcompeteui instance] setOrientation:fuelgameorientationportrait];
        }
    }
    
    BOOL iOSSetUserConditions(const char* data)
    {
        NSError *error = nil;
        NSString* conditionsString = [NSString stringWithFormat:@"%s" , data];
        NSData* conditionsData = [conditionsString dataUsingEncoding:NSUTF8StringEncoding];
        NSDictionary *conditions = [NSJSONSerialization JSONObjectWithData:conditionsData
                                                                   options:NSJSONReadingMutableContainers
                                                                     error:&error];
        
        if (error != nil) {
            return false;
        }
        
        conditions = (NSDictionary*)normalizeJSONDictionary(conditions);
        
        if (conditions == nil) {
            return false;
        }
        
        return [[fueldynamics instance] setUserConditions:conditions];
    }
    
    BOOL iOSSyncUserValues()
    {
        return [[fueldynamics instance] syncUserValues];
    }
    
    
    void iOSUseSandbox()
    {
        [fuel  useSandbox];
    }
    
    void iOSSetNotificationToken(const char* token)
    {
        [fuel setNotificationToken:[NSString stringWithUTF8String:token]];
    }
    
    void iOSSetOrientationuiCompete(const char* screenOrientation)
    {
        
        if (0 == strcmp(screenOrientation, "landscape")) {
            [[fuelcompeteui instance] setOrientation:fuelgameorientationlandscape];
            autoRotate = NO;
        } else if (0 == strcmp(screenOrientation, "portrait")) {
            [[fuelcompeteui instance] setOrientation:fuelgameorientationportrait];
            autoRotate = NO;
        } else {
            autoRotate = YES;
        }
    }
    
    void iOSSetOrientationuiIgnite(const char* screenOrientation)
    {
        
        if (0 == strcmp(screenOrientation, "landscape")) {
            [[fueligniteui instance] setOrientation:fuelgameorientationlandscape];
            autoRotate = NO;
        } else if (0 == strcmp(screenOrientation, "portrait")) {
            [[fueligniteui instance] setOrientation:fuelgameorientationportrait];
            autoRotate = NO;
        } else {
            autoRotate = YES;
        }
    }
    
    void iOSInitializeCompete()
    {
        [fuelcompete setup];
    }
    
    void iOSInitializeCompeteUI()
    {
        UIViewController* pUIViewController = UnityGetGLViewController();
        [fuelcompeteui setup:pUIViewController];
    }
    
    void iOSInitializeIgnite()
    {
        [fuelignite setup];
    }
    
    void iOSInitializeIgniteUI()
    {
        UIViewController* pUIViewController = UnityGetGLViewController();
        [fueligniteui setup:pUIViewController];
    }
    
    BOOL iOSExecMethod(const char* method, const char* params)
    {
        NSString* methodString = [NSString stringWithFormat:@"%s" , method];
        
        NSError *error = nil;
        NSString* paramsString = [NSString stringWithFormat:@"%s" , params];
        NSData* paramsData = [paramsString dataUsingEncoding:NSUTF8StringEncoding];
        NSArray* paramsArray= [NSJSONSerialization JSONObjectWithData:paramsData options:NSJSONReadingMutableContainers error:&error];
        
        if (error != nil) {
            return false;
        }
        
        fuelignite *gSDK = [fuelignite instance];
        return   [gSDK execMethod:methodString params:paramsArray];
    }
    
    void iOSSendProgress(const char* progress, const char* ruleTags)
    {
        NSError *error = nil;
        NSString* progressString = [NSString stringWithFormat:@"%s" , progress];
        NSData* progressData = [progressString dataUsingEncoding:NSUTF8StringEncoding];
        NSDictionary *progressDict = [NSJSONSerialization JSONObjectWithData:progressData
                                                                   options:NSJSONReadingMutableContainers
                                                                     error:&error];
        
        if (error != nil) {
            return;
        }
        
        progressDict = (NSDictionary*)normalizeJSONDictionary(progressDict);
        
        if (progressDict == nil) {
            return;
        }
        
        NSArray* ruleTagsArray = nil;
        if(ruleTags != nil) {
            NSString* ruleTagsString = [NSString stringWithFormat:@"%s" , ruleTags];
            NSData* ruleTagsData = [ruleTagsString dataUsingEncoding:NSUTF8StringEncoding];
            ruleTagsArray= [NSJSONSerialization JSONObjectWithData:ruleTagsData options:NSJSONReadingMutableContainers error:&error];
        }
        
        if (error != nil) {
            return;
        }
    
        fuelignite *gSDK = [fuelignite instance];
        [gSDK sendProgress:progressDict ruleTags:ruleTagsArray];
    }
    
    BOOL iOSIgniteGetEvent(const char* tags)
    {
        NSArray* tagsArray= nil;
        
        if( tags != nil ) {
            NSError *error = nil;
            NSString* tagsString = [NSString stringWithFormat:@"%s" , tags];
            NSData* tagsData = [tagsString dataUsingEncoding:NSUTF8StringEncoding];
            NSArray* tagsArray= [NSJSONSerialization JSONObjectWithData:tagsData options:NSJSONReadingMutableContainers error:&error];
        
            if (error != nil) {
                return false;
            }
        }
        
        fuelignite *gSDK = [fuelignite instance];
        return   [gSDK getEvents:tagsArray];
    }
    
    BOOL iOSIgniteGetLeaderBoard(const char* boardID)
    {
        NSString* boardIDString = [NSString stringWithFormat:@"%s" , boardID];
        
        fuelignite *gSDK = [fuelignite instance];
        return   [gSDK getLeaderBoard:boardIDString];
    }
    
    BOOL iOSIgniteGetMission(const char* missionID)
    {
        NSString* missionIDString = [NSString stringWithFormat:@"%s" , missionID];
        
        fuelignite *gSDK = [fuelignite instance];
        return   [gSDK getMission:missionIDString];
    }
    
    BOOL iOSIgniteGetQuest(const char* questID)
    {
        NSString* questIDString = [NSString stringWithFormat:@"%s" , questID];
        
        fuelignite *gSDK = [fuelignite instance];
        return   [gSDK getQuest:questIDString];
    }
    
    NSObject* normalizeJSONDictionary(NSDictionary *dictionary)
    {
        if (dictionary == nil) {
            return nil;
        }
        
        if (isNormalizedJSONValue(dictionary)) {
            return normalizeJSONValue(dictionary);
        }
        
        NSMutableDictionary *resultDictionary = [[NSMutableDictionary alloc] init];
        
        for (NSString *key in dictionary) {
            if (key == nil) {
                continue;
            }
            
            NSObject *value = [dictionary objectForKey:key];
            
            if (value == nil) {
                continue;
            }
            
            NSObject *normalizedValue = nil;
            
            if ([value isKindOfClass:[NSArray class]]) {
                normalizedValue = normalizeJSONList((NSArray*) value);
            } else if ([value isKindOfClass:[NSDictionary class]]) {
                normalizedValue = normalizeJSONDictionary((NSDictionary*) value);
            } else {
                continue;
            }
            
            if (normalizedValue == nil) {
                continue;
            }
            
            [resultDictionary setObject:normalizedValue forKey:key];
        }
        
        return resultDictionary;
    }
    
    NSObject* normalizeJSONList(NSArray *array)
    {
        if (array == nil) {
            return nil;
        }
        
        NSMutableArray *resultArray = [[NSMutableArray alloc] init];
        
        for (NSObject *value in array) {
            if (value == nil) {
                continue;
            }
            
            NSObject *normalizedValue = nil;
            
            if ([value isKindOfClass:[NSArray class]]) {
                normalizedValue = normalizeJSONList((NSArray*) value);
            } else if ([value isKindOfClass:[NSDictionary class]]) {
                normalizedValue = normalizeJSONDictionary((NSDictionary*) value);
            } else {
                continue;
            }
            
            if (normalizedValue == nil) {
                continue;
            }
            
            [resultArray addObject:normalizedValue];
        }
        
        return resultArray;
    }
    
    NSObject* normalizeJSONValue(NSDictionary *valueDictionary)
    {
        if (valueDictionary == nil) {
            return nil;
        }
        
        NSString *type = (NSString*) [valueDictionary objectForKey:@"type"];
        NSString *value = (NSString*) [valueDictionary objectForKey:@"value"];
        
        if ((type == nil) || (value == nil)) {
            return nil;
        }
        
        switch ([type intValue]) {
            case NORMALIZED_JSON_DATATYPE_INT:
            case NORMALIZED_JSON_DATATYPE_LONG:
            case NORMALIZED_JSON_DATATYPE_FLOAT:
            case NORMALIZED_JSON_DATATYPE_DOUBLE: {
                NSNumberFormatter *numberFormatter = [[NSNumberFormatter alloc] init];
                [numberFormatter setNumberStyle:NSNumberFormatterDecimalStyle];
                return [numberFormatter numberFromString:value];
            }
            case NORMALIZED_JSON_DATATYPE_BOOL: {
                BOOL boolValue = [value caseInsensitiveCompare:@"true"] == NSOrderedSame;
                return [NSNumber numberWithBool:boolValue];
            }
            case NORMALIZED_JSON_DATATYPE_STRING: {
                return value;
            }
            default:
                return nil;
        }
    }
    
    NSArray* normalizeList(NSString *valueString)
    {
        NSArray* valueArray = [valueString componentsSeparatedByString:@"|"];
        return valueArray;
    }
    
    BOOL isNormalizedJSONValue(NSDictionary *dictionary)
    {
        if (dictionary == nil) {
            return NO;
        }
        
        NSObject *checksumObject = [dictionary objectForKey:@"checksum"];
        
        if ((checksumObject == nil) || ![checksumObject isKindOfClass:[NSString class]]) {
            return NO;
        }
        
        NSString *checksum = (NSString*) checksumObject;
        
        if (![checksum isEqualToString:@"faddface"]) {
            return NO;
        }
        
        NSObject *typeObject = [dictionary objectForKey:@"type"];
        
        if ((typeObject == nil) || ![typeObject isKindOfClass:[NSString class]]) {
            return NO;
        }
        
        NSString *type = (NSString*) typeObject;
        
        switch ([type intValue]) {
            case NORMALIZED_JSON_DATATYPE_INT:
            case NORMALIZED_JSON_DATATYPE_LONG:
            case NORMALIZED_JSON_DATATYPE_FLOAT:
            case NORMALIZED_JSON_DATATYPE_DOUBLE:
            case NORMALIZED_JSON_DATATYPE_BOOL:
            case NORMALIZED_JSON_DATATYPE_STRING:
                break;
            default:
                return NO;
        }
        
        NSObject *valueObject = [dictionary objectForKey:@"value"];
        
        if ((valueObject == nil) || ![valueObject isKindOfClass:[NSString class]]) {
            return NO;
        }
        
        return YES;
    }
    
}

//
//  fuelignite.h
//  fuelsdk
//
//  Created by Alan Price on 2015-07-31.
//  Copyright (c) 2015 Fuel. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "fueltypes.h"

@interface fuelignite : NSObject

+ (void)setup;
+ (fuelignite*)instance;


- (bool)execMethod:(NSString *)method params:(NSArray *)params;

- (bool)sendProgress:(NSDictionary *)progress ruleTags:(NSArray *)ruleTags;
- (bool)getEvents:(NSArray *)eventTags;
- (bool)joinEvent:(NSString *)eventID;
- (bool)getLeaderBoard:(NSString *)boardID;
- (bool)getMission:(NSString *)missionID;
- (bool)getQuest:(NSString *)questID;

@end

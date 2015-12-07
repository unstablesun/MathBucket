//
//  fuelcompete.h
//  fuelsdk
//
//  Created by Alan Price on 2015-07-31.
//  Copyright (c) 2015 Fuel. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "fueltypes.h"

@interface fuelcompete : NSObject

+ (void)setup;
+ (fuelcompete*)instance;

- (BOOL)submitMatchResult:(NSDictionary*)matchResult;
- (void)syncChallengeCounts;
- (void)syncTournamentInfo;


@end

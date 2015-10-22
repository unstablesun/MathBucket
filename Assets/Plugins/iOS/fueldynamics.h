//
//  fueldynamics.h
//  fuelsdk
//
//  Created by Alan Price on 2015-07-31.
//  Copyright (c) 2015 Fuel. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "fueltypes.h"

@interface fueldynamics : NSObject

+ (void)setup;
+ (fueldynamics*)instance;

- (BOOL)setUserConditions:(NSDictionary *)conditions;
- (BOOL)syncUserValues;

@end

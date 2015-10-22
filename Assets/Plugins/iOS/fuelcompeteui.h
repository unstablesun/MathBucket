//
//  fuelcompeteui.h
//  fuelsdk
//
//  Created by Alan Price on 2015-07-31.
//  Copyright (c) 2015 Fuel. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import "fueltypes.h"

@interface fuelcompeteui : NSObject

+ (void)setup:(UIViewController*)rootViewController;
+ (fuelcompeteui*)instance;

- (void)setOrientation:(fuelgameorientation)orientation;
- (BOOL)launch;


@end

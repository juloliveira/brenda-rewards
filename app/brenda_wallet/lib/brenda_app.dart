import 'package:brenda_wallet/routes/brenda_password_route.dart';
import 'package:brenda_wallet/routes/brenda_receive.dart';
import 'package:brenda_wallet/routes/brenda_send.dart';
import 'package:brenda_wallet/routes/brenda_shopping.dart';
import 'package:brenda_wallet/routes/brenda_sign_in.dart';
import 'package:brenda_wallet/routes/brenda_sign_up_wizard.dart';
import 'package:brenda_wallet/routes/brenda_wallet.dart';
import 'package:brenda_wallet/routes/brenda_welcome.dart';
import 'package:brenda_wallet/splashscreen.dart';
import 'package:flutter/material.dart';
import 'package:flutter_flavor/flutter_flavor.dart';

import 'data/my_colors.dart';

class BrendaApp {
  static Widget init() {
    return FlavorBanner(
        child: MaterialApp(
      debugShowCheckedModeBanner: false,
      theme: ThemeData(
          primaryColor: MyColors.primary,
          accentColor: MyColors.accent,
          primaryColorDark: MyColors.primaryDark,
          primaryColorLight: MyColors.primaryLight,
          bottomSheetTheme:
              BottomSheetThemeData(backgroundColor: Colors.transparent)),
      home: SplashScreen(),
      routes: <String, WidgetBuilder>{
        //'/MenuRoute': (BuildContext context) => new MenuRoute(),
        //'/About': (BuildContext context) => new AboutAppRoute(),
        '/BrendaSignIn': (BuildContext context) => new BrendaSignInRoute(),
        '/BrendaWallet': (BuildContext context) => new BrendaWalletRoute(),
        '/BrendaShopping': (BuildContext context) => new BrendaShoppingRoute(),
        '/BrendaSignUpWizard': (BuildContext context) =>
            new BrendaSignUpWizardRoute(),
        '/BrendaWelcome': (BuildContext context) => new BrendaWelcomeRoute(),
        '/BrendaSend': (BuildContext context) => new BrendaSendRoute(),
        '/BrendaReceive': (BuildContext context) => new BrendaReceiveRoute(),
        '/BrendaPassword': (BuildContext context) => new BrendaPasswordRoute()
      },
    ));
  }
}

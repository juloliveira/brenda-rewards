import 'dart:core';

import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:brenda_wallet/data/img.dart';
import 'package:brenda_wallet/data/my_colors.dart';
import 'package:brenda_wallet/data/my_strings.dart';
import 'package:brenda_wallet/widget/my_text.dart';

class BrendaWelcomeRoute extends StatefulWidget {
  BrendaWelcomeRoute();

  @override
  BrendaWelcomeRouteState createState() => new BrendaWelcomeRouteState();
}

class BrendaWelcomeRouteState extends State<BrendaWelcomeRoute>
    with WidgetsBindingObserver {
  // AppLifecycleState _lastLifecycleState;

  // @override
  // void initState() {
  //   super.initState();
  //   WidgetsBinding.instance.addObserver(this);
  // }

  // @override
  // void dispose() {
  //   WidgetsBinding.instance.removeObserver(this);
  //   super.dispose();
  // }

  // @override
  // void didChangeAppLifecycleState(AppLifecycleState state) {
  //   setState(() {
  //     _lastLifecycleState = state;
  //   });
  // }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: MyColors.grey_10,
      appBar: PreferredSize(
          preferredSize: Size.fromHeight(0),
          child: Container(color: Colors.black)),
      body: SingleChildScrollView(
        child: Container(
          child: Column(
            children: <Widget>[
              Stack(
                children: <Widget>[
                  Container(
                    width: double.infinity,
                    height: 256,
                    foregroundDecoration: BoxDecoration(
                        color: Colors.blueAccent[400].withOpacity(0.4)),
                    child:
                        Image.asset(Img.get('image_8.jpg'), fit: BoxFit.cover),
                  ),
                  Container(
                    width: double.infinity,
                    height: 256,
                    child: Column(
                      mainAxisSize: MainAxisSize.min,
                      mainAxisAlignment: MainAxisAlignment.center,
                      children: <Widget>[
                        Image.asset(Img.get('logo_small.png'),
                            width: 80, height: 80),
                        Text("Bem-vindo à Brenda",
                            style: MyText.title(context)
                                .copyWith(color: Colors.white))
                      ],
                    ),
                  ),
                  Row(children: <Widget>[
                    IconButton(
                      icon: Icon(Icons.menu, color: Colors.white),
                      onPressed: () {
                        Navigator.pop(context);
                      },
                    ),
                    Spacer(),
                    IconButton(
                      icon: Icon(Icons.search, color: Colors.white),
                      onPressed: () {},
                    ),
                    IconButton(
                      icon: Icon(Icons.more_vert, color: Colors.white),
                      onPressed: () {},
                    ),
                  ]),
                ],
              ),
              Container(
                padding: EdgeInsets.all(10),
                transform: Matrix4.translationValues(0.0, -50.0, 0.0),
                child: Column(
                  children: <Widget>[
                    Card(
                      shape: RoundedRectangleBorder(
                        borderRadius: BorderRadius.circular(2),
                      ),
                      color: Colors.white,
                      elevation: 2,
                      clipBehavior: Clip.antiAliasWithSaveLayer,
                      child: Container(
                        padding:
                            EdgeInsets.symmetric(vertical: 15, horizontal: 15),
                        child: Column(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: <Widget>[
                            Text("Agora você poderá:",
                                style: MyText.title(context)
                                    .copyWith(color: MyColors.grey_80)),
                            Container(height: 25),
                            Row(
                              children: <Widget>[
                                Icon(Icons.local_cafe, color: MyColors.grey_40),
                                Container(width: 10),
                                Text("Ganhar recompensas",
                                    style: MyText.subhead(context)
                                        .copyWith(color: MyColors.grey_40))
                              ],
                            ),
                            Container(height: 10),
                            Row(
                              children: <Widget>[
                                Icon(Icons.local_florist,
                                    color: MyColors.grey_40),
                                Container(width: 10),
                                Text("Comprar com a moeda Brenda",
                                    style: MyText.subhead(context)
                                        .copyWith(color: MyColors.grey_40))
                              ],
                            ),
                            Container(height: 10),
                            Row(
                              children: <Widget>[
                                Icon(Icons.airport_shuttle,
                                    color: MyColors.grey_40),
                                Container(width: 10),
                                Text("Vender com a moeda Brenda",
                                    style: MyText.subhead(context)
                                        .copyWith(color: MyColors.grey_40))
                              ],
                            ),
                            Container(height: 10),
                            Row(
                              children: <Widget>[
                                Icon(Icons.beach_access,
                                    color: MyColors.grey_40),
                                Container(width: 10),
                                Text("Muito mais",
                                    style: MyText.subhead(context)
                                        .copyWith(color: MyColors.grey_40))
                              ],
                            ),
                            Container(height: 10),
                          ],
                        ),
                      ),
                    ),
                    Container(height: 5),
                    Card(
                      shape: RoundedRectangleBorder(
                        borderRadius: BorderRadius.circular(2),
                      ),
                      color: Colors.white,
                      elevation: 2,
                      clipBehavior: Clip.antiAliasWithSaveLayer,
                      child: Container(
                        padding:
                            EdgeInsets.symmetric(vertical: 15, horizontal: 15),
                        child: Column(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: <Widget>[
                            Text("Próximos passos:",
                                style: MyText.title(context)
                                    .copyWith(color: MyColors.grey_80)),
                            Container(height: 10),
                            Text(MyStrings.welcome,
                                style: MyText.subhead(context)
                                    .copyWith(color: MyColors.grey_40)),
                            Container(height: 5),
                          ],
                        ),
                      ),
                    )
                  ],
                ),
              )
            ],
          ),
        ),
      ),
    );
  }
}

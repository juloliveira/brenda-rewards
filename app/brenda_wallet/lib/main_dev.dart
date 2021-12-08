import 'dart:io';

import 'package:flutter/material.dart';
import 'package:flutter_flavor/flutter_flavor.dart';
import 'brenda_app.dart';
import 'helpers/http_overrides.dart';

void main() {
  HttpOverrides.global = new MyHttpOverrides();

  FlavorConfig(
      environment: FlavorEnvironment.DEV,
      name: "DEVEL",
      color: Colors.red,
      location: BannerLocation.bottomStart,
      variables: {
        "user": "jul.oliveira@gmail.com",
        "pass": "JFPjro%2013",
        "sara.api": 'https://192.168.15.18:8421',
        "carol.api": 'https://192.168.15.18:8420'
      });

  runApp(BrendaApp.init());
}

class BrendaRoute {
  final String route;
  final List<Map<String, dynamic>> user;

  BrendaRoute(this.route, {this.user});
}

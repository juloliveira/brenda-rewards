import 'dart:io';

import 'package:flutter/material.dart';
import 'package:flutter_flavor/flutter_flavor.dart';
import 'brenda_app.dart';
import 'helpers/http_overrides.dart';

void main() {
  HttpOverrides.global = new MyHttpOverrides();

  FlavorConfig(
      environment: FlavorEnvironment.TEST,
      name: "TEST",
      color: Colors.deepPurpleAccent,
      location: BannerLocation.bottomStart,
      variables: {
        "user": "",
        "pass": "",
        "sara.api": 'http://129.213.171.238',
        "carol.api": 'http://129.213.191.254'
      });

  runApp(BrendaApp.init());
}

class BrendaRoute {
  final String route;
  final List<Map<String, dynamic>> user;

  BrendaRoute(this.route, {this.user});
}

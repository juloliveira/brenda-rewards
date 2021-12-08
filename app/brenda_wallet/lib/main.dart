import 'dart:io';

import 'package:flutter/material.dart';
import 'package:flutter_flavor/flutter_flavor.dart';
import 'brenda_app.dart';
import 'helpers/http_overrides.dart';

void main() {
  HttpOverrides.global = new MyHttpOverrides();

  FlavorConfig(variables: {
    "user": "",
    "pass": "",
    "sara.api": 'https://sara.brendarewards.com',
    "carol.api": 'https://carol.brendarewards.com',
  });
  runApp(BrendaApp.init());
}

class BrendaRoute {
  final String route;
  final List<Map<String, dynamic>> user;

  BrendaRoute(this.route, {this.user});
}

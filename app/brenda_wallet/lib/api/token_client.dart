import 'dart:convert';

import 'package:flutter_flavor/flutter_flavor.dart';
import 'package:http/http.dart' as http;

class TokenClient {
  Future<dynamic> signIn(String user, String pass) async {
    final url =
        '${FlavorConfig.instance.variables["sara.api"]}/security/sign-in';
    final jsonUser = jsonEncode(
        {'username': user, 'password': pass, 'grant_type': 'password'});
    final response = await http.post(url,
        body: jsonUser, headers: {'Content-type': 'application/json'});

    if (response.statusCode != 200)
      throw new Exception('Acesso não foi autorizado');

    final json = jsonDecode(response.body);
    return json;
  }

  Future<dynamic> refreshToken(String username, String refreshToken) async {
    final url =
        '${FlavorConfig.instance.variables["sara.api"]}/security/sign-in';

    final jsonUser = jsonEncode({
      'username': username,
      'refresh_token': refreshToken,
      'grant_type': 'refresh_token'
    });
    final response = await http.post(url,
        body: jsonUser, headers: {'Content-type': 'application/json'});

    if (response.statusCode != 200) throw new Exception('error getting quotes');

    final json = jsonDecode(response.body);
    print("REFRESH TOKEN CALLED !");
    return json;
  }
}

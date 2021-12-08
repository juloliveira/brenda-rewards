import 'dart:convert';

import 'package:brenda_wallet/models/user_register.dart';
import 'package:flutter_flavor/flutter_flavor.dart';
import 'package:http/http.dart' as http;
import 'api_client.dart';

class UserClient extends ApiClient {
  Future updateFirebaseToken(String token) async {
    final url = '${FlavorConfig.instance.variables["sara.api"]}/token';

    var response = await http.put(url,
        body: jsonEncode({'token': token}), headers: await headers(true));
    if (response.statusCode != 200) {
      print("Status Code: ${response.statusCode} Message: ${response.body}");
    }
  }

  Future<dynamic> registerUser(UserRegister userRegister) async {
    final url =
        '${FlavorConfig.instance.variables["sara.api"]}/security/create';
    final jsonUser = jsonEncode(userRegister);
    final response =
        await http.post(url, body: jsonUser, headers: await headers(false));

    if (response.statusCode != 200) throw new Exception('error getting quotes');

    final json = jsonDecode(response.body);
    return json;
  }

  Future requestNewPassword(String email) async {
    final url =
        '${FlavorConfig.instance.variables["sara.api"]}/security/recover';
    final jsonUser = jsonEncode({"email": email, "token": ""});
    final response =
        await http.post(url, body: jsonUser, headers: await headers(false));

    if (response.statusCode != 200) throw new Exception('error getting quotes');

    return;
  }
}

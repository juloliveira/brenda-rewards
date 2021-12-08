import 'dart:io';

import 'package:brenda_wallet/api/token_client.dart';
import 'package:brenda_wallet/contants/keys.dart';
import 'package:brenda_wallet/database/database.dart';
import 'package:brenda_wallet/exceptions/campaign_rewarded.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';

class ApiClient {
  final _storage = FlutterSecureStorage();

  Future<Map<String, String>> headers(bool auth) async {
    if (auth) {
      var expirationData = await _storage.read(key: Keys.TOKEN_EXPIRATION);
      if (expirationData == null) {
        throw new RefreshTokenUnauthorizedException();
      } else {
        var expiration = DateTime.parse(expirationData + 'Z');
        if (expiration.difference(DateTime.now().toUtc()).inSeconds < 0) {
          var username = await _storage.read(key: Keys.USERNAME);
          var refreshToken = await _storage.read(key: Keys.REFRESH_TOKEN);
          var token = await TokenClient().refreshToken(username, refreshToken);
          await Db().saveToken(username, token);
          return await this.headers(auth);
        }
      }
    }

    Map<String, String> headers;
    if (auth) {
      var accessToken = await _storage.read(key: Keys.ACCESS_TOKEN);
      headers = {
        HttpHeaders.authorizationHeader: 'Bearer ' + accessToken,
        'Content-type': 'application/json',
        'Accept': 'application/json'
      };
    } else {
      headers = {'Content-type': 'application/json'};
    }

    return headers;
  }
}

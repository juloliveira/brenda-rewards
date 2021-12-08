import 'package:brenda_wallet/contants/keys.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';

class Db {
  final _storage = FlutterSecureStorage();

  Future clear() async {
    await _storage.deleteAll();
  }

  Future saveToken(String username, Map<String, dynamic> token) async {
    await _storage.write(key: Keys.USER_ID, value: token['user_id']);
    await _storage.write(key: Keys.USERNAME, value: username);
    await _storage.write(key: Keys.ACCESS_TOKEN, value: token['access_token']);
    await _storage.write(
        key: Keys.REFRESH_TOKEN, value: token['refresh_token']);
    await _storage.write(key: Keys.TOKEN_CREATED, value: token['created']);
    await _storage.write(
        key: Keys.TOKEN_EXPIRATION, value: token['expiration']);
  }

  Future saveMap(String prefix, Map<String, dynamic> map) async {
    map.forEach((key, value) async =>
        await _storage.write(key: '${prefix}_$key', value: value));
  }

  Future save(String prefix, String key, String value) async {
    await _storage.write(key: '${prefix}_$key', value: value);
  }

  Future<String> key(String key) async {
    return _storage.read(key: key);
  }
}

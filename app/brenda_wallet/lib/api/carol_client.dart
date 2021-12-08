import 'dart:convert';

import 'package:brenda_wallet/api/api_client.dart';
import 'package:brenda_wallet/models/user.dart';
import 'package:brenda_wallet/models/user_send.dart';
import 'package:brenda_wallet/models/user_transfer_brenda.dart';
import 'package:flutter_flavor/flutter_flavor.dart';
import 'package:http/http.dart' as http;

class CarolClient extends ApiClient {
  Future<User> getUserData() async {
    final url = '${FlavorConfig.instance.variables["carol.api"]}/account';

    var response = await http.get(url, headers: await headers(true));

    if (response.statusCode != 200) {
      throw new Exception('Erro ao acessar dados do seu usuário');
    }

    var jsonObject = json.decode(response.body)['result']['data'];
    var user =
        User(jsonObject['id'], jsonObject['balance'], jsonObject['email']);

    user.transactions = List<dynamic>.from(jsonObject['transactions'])
        .map((item) => Transaction.fromJson(item))
        .toList();

    return user;
  }

  Future<UserSend> getUserSend(String userId) async {
    final url = '${FlavorConfig.instance.variables["carol.api"]}/send';
    var response = await http.post(url,
        body: jsonEncode({'user_id': userId}), headers: await headers(true));

    if (response.statusCode != 200) {
      throw new Exception('Erro ao acessar dados do usuário');
    }

    var json = jsonDecode(response.body)['result']['data'];

    return UserSend(json['id'], json['email'], json['phone_number']);
  }

  Future<UserTransferBrenda> postUserTransfer(
      UserSend userSend, double userTransferValue) async {
    final url = '${FlavorConfig.instance.variables["carol.api"]}/transfer';

    var body = jsonEncode({
      'to_user_id': userSend.id,
      'to_user_email': userSend.email,
      'to_user_phone': userSend.phoneNumber,
      'value': userTransferValue
    });

    var response =
        await http.put(url, body: body, headers: await headers(true));

    if (response.statusCode != 200) {
      throw new Exception("Erro ao executar transferência.");
    }

    //var json = jsonDecode(response.body)['result']['data'];

    return UserTransferBrenda("Foi");
  }
}

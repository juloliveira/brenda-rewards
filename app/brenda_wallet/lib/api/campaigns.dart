import 'dart:convert';
import 'package:brenda_wallet/exceptions/campaign_rewarded.dart';
import 'package:brenda_wallet/models/brenda_gps.dart';
import 'package:brenda_wallet/models/reward_message.dart';
import 'package:brenda_wallet/models/voucher.dart';
import 'package:flutter_flavor/flutter_flavor.dart';

import 'package:http/http.dart' as http;

import 'api_client.dart';

class CampaignClient extends ApiClient {
  Future<Voucher> getCampaign(
      BrendaTag brendaTag, String deviceId, String deviceData) async {
    var tag = brendaTag.tag;
    if (tag.contains('http')) {
      tag = tag.replaceAll('https://', '');
      tag = tag.replaceAll('/', '-');
      tag = '67685586';
    }

    final url =
        '${FlavorConfig.instance.variables["sara.api"]}/campaign/${tag}';

    var message = jsonEncode({
      'tag': tag,
      'lat': brendaTag.gps.latitude,
      'lng': brendaTag.gps.longitude,
      'acc': brendaTag.gps.accuracy,
      'alt': brendaTag.gps.altitude,
      'spe': brendaTag.gps.speed,
      'spa': brendaTag.gps.speedAccuracy,
      'hea': brendaTag.gps.heading,
      'did': deviceId,
      'dev': deviceData
    });

    var response =
        await http.post(url, body: message, headers: await headers(true));

    if (response.statusCode != 200) {
      var json = jsonDecode(response.body);
      throw new ApiException(response.statusCode, json['title']);
    }

    var voucher = Voucher(jsonDecode(response.body));

    return voucher;
  }

  Future<RewardMessage> getReward(Voucher voucher) async {
    final url =
        '${FlavorConfig.instance.variables["sara.api"]}/campaign/${voucher.campaign.id}/voucher/${voucher.id}/reward';
    var message = jsonEncode({
      'voucher_id': voucher.id,
      'campaign_id': voucher.campaign.id,
      'replies': voucher.campaign.replies
    });
    var response =
        await http.put(url, body: message, headers: await headers(true));

    if (response.statusCode != 200) {
      throw new Exception('error getting reward');
    }

    var json = jsonDecode(response.body)['result']['data'];
    var reward = RewardMessage(json['cs'], json['lo'], json['re']);

    return reward;
  }
}

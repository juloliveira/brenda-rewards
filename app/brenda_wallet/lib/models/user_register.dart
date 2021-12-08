import 'package:brenda_wallet/helpers/datetime.dart';

class UserRegister {
  final String email;

  final String document;
  final String phoneNumber;
  final String birthdate;

  final double latitude;
  final double longitude;

  final int sex;
  final String educationLevelId;
  final String genderIdentityId;
  final String incomeId;
  final String sexualityId;

  final String deviceId;
  final String deviceData;

  final String token;

  const UserRegister(
      {this.email,
      this.document,
      this.phoneNumber,
      this.birthdate,
      this.sex,
      this.genderIdentityId,
      this.incomeId,
      this.sexualityId,
      this.educationLevelId,
      this.latitude,
      this.longitude,
      this.deviceId,
      this.deviceData,
      this.token});

  Map toJson() => {
        'email': this.email,
        'document': this.document,
        'phone_number': this.phoneNumber,
        'birthdate': toRequest(this.birthdate),
        'latitude': this.latitude,
        'longitude': this.longitude,
        'sex': this.sex,
        'education_level_id': this.educationLevelId,
        'gender_identity_Id': this.genderIdentityId,
        'income_id': this.incomeId,
        'sexuality_id': this.sexualityId,
        'device_id': this.deviceId,
        'device_data': this.deviceData,
        'token': this.token
      };
}

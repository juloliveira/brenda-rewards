class ApiException implements Exception {
  final int statusCode;
  final String message;

  const ApiException(this.statusCode, this.message);
}

mixin CampaignRewardedException implements Exception {}

class RefreshTokenUnauthorizedException implements Exception {}

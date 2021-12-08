class BrendaGps {
  double latitude;
  double longitude;
  double accuracy;
  double altitude;
  double speed;
  double speedAccuracy;
  double heading;

  BrendaTag withTag(String tag) => BrendaTag(tag, this);
}

class BrendaTag {
  final String tag;
  final BrendaGps gps;

  BrendaTag(this.tag, this.gps);
}

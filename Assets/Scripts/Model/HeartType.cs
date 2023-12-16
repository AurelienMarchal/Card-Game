public enum HeartType{
    NoHeart, Red, RedEmpty, Blue, Nature, Cursed
}

public static class HeartTypeExtensions{
    public static string ToString(this HeartType heartType)
    {
        switch(heartType){
            case HeartType.NoHeart: return "NoHeart";
            case HeartType.Red: return "RedHeart";
            case HeartType.RedEmpty: return "RedEmpty";
            case HeartType.Blue: return "BlueHeart";
            case HeartType.Nature: return "NatureHeart";
            case HeartType.Cursed: return "CursedHeart";
            default: return "NoHeart";
        }
    }
}


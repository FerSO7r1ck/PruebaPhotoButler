using UnityEngine;

public static class GlobalDeclarations
{
    public static string ChromaKey = "#00FF00FF";
    public static bool EnableTestMode = false;
    public static bool IsRunningAsAServer = false;
    public static bool IsPcBuild = false;

    public enum AnimationType
    {
        Unknown,
        XRotation,
        YRotation,
        ZRotation,
        Scale,
        Translation,
        Orientation,
        Rotation,
        Zoom
    }

    public enum TemplatesFolderNames
    {
        TestTemplate,
        BreakingMatzo_AfikoMan,
        BreakingMatzo_HoraChair,
        BreakingMatzo_SederPlate,
        CarnivalCruises,
        ClubMed_Cancun,
        DisneyDayAtDisneyland_Unity,
        DisneyHauntedMansion_Unity,
        DisneyMagicHappens_Unity,
        DisneyWelcomeFromWalt_Unity,
        GupShup_AirAsia,
        GupShup_BollywoodBirthday,
        GupShup_Cricket,
        MSC_Dining,
        MSC_MagnificaItinerary,
        msc_magnificaitinerary_Front,
        TestTemplate_Portrait,
        TestTemplate_Video_Transparent,
        TestTemplate_Video_Transparent_Paul,
        Test_MickeyVertical_Paul,
        Test_DisneyHoop_Vertical_Dan
    }

    public enum UGCIndex
    {
        UGC1,
        UGC2,
        UGC3,
        UGC4,
        UGC5,
        UGC6,
        UGC7,
        UGC8,
        UGC9,
        UGC10,
        UGC11,
        UGC12
    }

    public enum TemplateType
    {
        Landscape,
        Portrait,
        Square
    }

    public enum UGCPositionType
    {
        Back,
        Front
    }

    public enum MovieObjectType
    {
        Image,
        Video,
        Text
    }

    public enum CallbackSource
    {
        Internal,
        External
    }
}

public class UGCSpotlight 
{
    public string UGCId;
    public int Frame;
    public Vector3 Position;
    public Vector3 Scale;
    public float Width;
    public float Height;
}
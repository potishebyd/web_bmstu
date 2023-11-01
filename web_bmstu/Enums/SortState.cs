namespace web_bmstu.Enums
{
    public enum SongSortState
    {
        IdAsc,
        IdDesc,

        TitleAsc,
        TitleDesc,

        AlbumAsc,
        AlbumDesc,

        GenreAsc,
        GenreDesc,

        ArtistNameAsc,
        ArtistNameDesc,

        RecordingStudioNameAsc,
        RecordingStudioNameDesc,

        DurationAsc,
        DurationDesc
    }

    public enum ArtistSortState
    {
        IdAsc,
        IdDesc,

        NameAsc,
        NameDesc,

        CountryAsc,
        CountryDesc
    }

    public enum RecordingStudioSortState
    {
        IdAsc,
        IdDesc,

        NameAsc,
        NameDesc,

        CountryAsc,
        CountryDesc,

        YearFoundedAsc,
        YearFoundedDesc
    }

    public enum UserSortState
    {
        IdAsc,
        IdDesc,

        LoginAsc,
        LoginDesc,

        PermissionAsc,
        PermissionDesc,

        EmailAsc,
        EmailDesc,

        NamePlaylistAsc,
        NamePlaylistDesc
    }

    public enum PlaylistSortState
    {
        IdAsc,
        IdDesc,

        DurationAsc,
        DurationDesc,

        NameAsc,
        NameDesc,

        CreationDateAsc,
        CreationDateDesc
    }
}

namespace VolleyLeague.Shared.Dtos.Matches
{
    public class VenueDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public string? AdditionalInfo { get; set; }

        //public static explicit operator VenueDto(SportsVenue venue)
        //{
        //    return new VenueDto
        //    {
        //        Id = venue.Id,
        //        Name = venue.Name,
        //        AdditionalInfo = venue.AdditionalInfo
        //    };
        //}

        //public static explicit operator SportsVenue(VenueDto venueDto)
        //{
        //    return new SportsVenue    
        //    {
        //        Id = venueDto.Id,
        //        Name = venueDto.Name,
        //        AdditionalInfo = venueDto.AdditionalInfo
        //    };
        //}
    }
}

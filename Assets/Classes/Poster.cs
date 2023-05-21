
public class Poster
{
    private string posterName;
    private string fileUrl;
    private int roomId;
    private int userId;
    private int posterId;
    private Position position;

    public Poster(PosterDTO posterDto)
    {
        this.posterName = posterDto.posterName;
        this.fileUrl = posterDto.fileUrl;
        this.roomId = posterDto.roomId;
        this.userId = posterDto.userId;
        this.posterId = posterDto.posterId;
        position = new Position(posterDto.position);
    }
}

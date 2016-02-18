public class ShortestPathStep {

	public Tile position;
	public int gScore;
	public int hScore;
	public ShortestPathStep parent;

	public ShortestPathStep(Tile t) {
		position = t;
		gScore = 0;
		hScore = 0;
		parent = null;
	}
	
	public int Fscore() {
		return gScore + hScore;
	}
}

public class ShortestPathStep {

	int gScore;
	int hScore;
	ShortestPathStep parent;

	public ShortestPathStep() {
		gScore = 0;
		hScore = 0;
		parent = null;
	}
	
	public int Fscore() {
		return gScore + hScore;
	}
}

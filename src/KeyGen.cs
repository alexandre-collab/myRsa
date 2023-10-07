namespace src;

class Keygen 
{

    private string key;
    private string fileName;
    private int size;

    /// <summary>
    /// Constructor for the Keygen class.
    /// </summary>
    /// <param name="key">The key to generate.</param>
    /// <param name="fileName">File name (optional, default is "monRSA").</param>
    /// <param name="size">The size of the key (optional, default is 10).</param>
    public Keygen (string key, string fileName = "monRSA", int size = 10)
    {
        this.key = key;
        this.fileName = fileName;
        this.size = size;
    }

    public void generateKey()
    {
        // todo 
    }

}
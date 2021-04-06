using UnityEngine;
using UnityEngine.UIElements;

public class SpawnPlatform : MonoBehaviour
{
    public GameObject platform;

    int numPlf = 1;

    // platform pattern
    int ptnIndex = 0;
    int[] size = {12, 16, 20, 24, 28};

    struct PLF_PTN
    {
        public int ptnIndex;
        public Vector2[] posPtn;
        public SIZE_PTN sizePtn;
    };

    struct SIZE_PTN
    {
        public int[] bias;
        public float[] rate;
    };

    PLF_PTN[][] dif;

    // last spawned platform
    struct PLATFORM
    {
        public int size;
        public float z;
        public Vector2 xy;
    };

    PLATFORM plf;

    public void OnPlatformDestroyed()
    {
        GeneratePlf();
    }

    void Start()
    {
        // initialize platform pattern
        dif = new PLF_PTN[][]
        {
            new PLF_PTN[] // very easy
            {
                InitPlfPattern(V2s(V2(-2, 1), V2(2, 1)), 0f, Int(0), Flt(1f)),
                InitPlfPattern(V2s(V2(-2, -1), V2(2, -1)), 0f, Int(0), Flt(1f)),
                InitPlfPattern(V2s(V2(-1, -2), V2(1, -2)), 0f, Int(0), Flt(1f))
            },

            new PLF_PTN[] // easy
            {
                InitPlfPattern(V2s(V2(-1, 3), V2(1, 3)), 0f, Int(0), Flt(1f)),
                InitPlfPattern(V2s(V2(-2, 2), V2(2, 2)), 0f, Int(0), Flt(1f)),
                InitPlfPattern(V2s(V2(-3, 1), V2(3, 1)), 0f, Int(0), Flt(1f)),
                InitPlfPattern(V2s(V2(-4, 0), V2(-3, 0), V2(3, 0), V2(4, 0)), 0f, Int(0), Flt(1f)),
                InitPlfPattern(V2s(V2(-4, -1), V2(-3, -1), V2(3, -1), V2(4, -1)), 0f, Int(0), Flt(1f)),
                InitPlfPattern(V2s(V2(-4, -2), V2(-3, -2), V2(-2, -2), V2(2, -2), V2(3, -2), V2(4, -2)), 0f, Int(0), Flt(1f)),
                InitPlfPattern(V2s(V2(-3, -3), V2(-2, -3), V2(-1, -3), V2(1, -3), V2(2, -3), V2(3, -3)), 0f, Int(0), Flt(1f)),
                InitPlfPattern(V2s(V2(-2, -4), V2(-1, -4), V2(1, -4), V2(2, -4)), 0f, Int(0), Flt(1f)),
            },

            new PLF_PTN[] // medium
            {
                InitPlfPattern(V2s(V2(-2, 3), V2(2, 3)), 0f, Int(0), Flt(1f)),
                InitPlfPattern(V2s(V2(-3, 2), V2(3, 2)), 0f, Int(0), Flt(1f)),
                InitPlfPattern(V2s(V2(-4, 1), V2(4, 1)), 0f, Int(0), Flt(1f)),
                InitPlfPattern(V2s(V2(-5, -1), V2(5, -1)), 0f, Int(0), Flt(1f)),
                InitPlfPattern(V2s(V2(-5, -2), V2(5, -2)), 0f, Int(0), Flt(1f)),
                InitPlfPattern(V2s(V2(-5, -3), V2(-4, -3), V2(4, -3), V2(5, -3)), 0f, Int(0), Flt(1f)),
                InitPlfPattern(V2s(V2(-5, -4), V2(-4, -4), V2(-3, -4), V2(3, -4), V2(4, -4), V2(5, -4)), 0f, Int(0), Flt(1f)),
            },

            new PLF_PTN[] // hard
            {
                InitPlfPattern(V2s(V2(-1, 4), V2(1, 4)), 0f, Int(0), Flt(1f)),
                InitPlfPattern(V2s(V2(-3, 3), V2(3, 3)), 0f, Int(0), Flt(1f)),
                InitPlfPattern(V2s(V2(-4, 2), V2(4, 2)), 0f, Int(0), Flt(1f)),
                InitPlfPattern(V2s(V2(-5, 1), V2(5, 1)), 0f, Int(0), Flt(1f)),
                InitPlfPattern(V2s(V2(-5, 0), V2(5, 0)), 0f, Int(0), Flt(1f)),
                InitPlfPattern(V2s(V2(-6, -1), V2(6, -1)), 0f, Int(0), Flt(1f)),
                InitPlfPattern(V2s(V2(-6, -2), V2(6, -2)), 0f, Int(0), Flt(1f)),
                InitPlfPattern(V2s(V2(-6, -3), V2(6, -3)), 0f, Int(0), Flt(1f)),
                InitPlfPattern(V2s(V2(-6, -4), V2(6, -4)), 0f, Int(0), Flt(1f)),
            },

            new PLF_PTN[] // very hard
            {
                InitPlfPattern(V2s(V2(-3, 4), V2(-2, 4), V2(2, 4), V2(3, 4)), 0f, Int(0), Flt(1f)),
                InitPlfPattern(V2s(V2(-4, 3), V2(4, 3)), 0f, Int(0), Flt(1f)),
                InitPlfPattern(V2s(V2(-5, 2), V2(5, 2)), 0f, Int(0), Flt(1f)),
                InitPlfPattern(V2s(V2(-7, -3), V2(7, -3)), 0f, Int(0), Flt(1f)),
                InitPlfPattern(V2s(V2(-7, -4), V2(7, -4)), 0f, Int(0), Flt(1f)),
            },
        };

        // initialize platform data
        plf = new PLATFORM();

        plf.size = 50;
        plf.z = 25f;
        plf.xy = new Vector2(0, 0);

        // generate 4 more platforms
        for (int i = 0; i < 8; i++)
        {
            GeneratePlf();
        }

        // update platform
        FindObjectOfType<GameOver>().OnPlatformUpdate();
    }

    void GeneratePlf()
    {
        // begin with altering previous information
        plf.z += (float)plf.size / 2; 
        plf.size = size[Random.Range(0, size.Length)];
        plf.xy += ChoosePattern();
        plf.z += (float)plf.size / 2;

        // generate new platform
        GameObject obj = Instantiate(platform, transform.position, Quaternion.identity);

        //obj.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        obj.transform.localScale = new Vector3(1f, 1f, plf.size);
        obj.transform.position = new Vector3(plf.xy.x, plf.xy.y, plf.z);
        obj.transform.parent = transform;
        obj.name = numPlf.ToString();
        obj.tag = "Platforms";

        numPlf++;
    }

    Vector2 ChoosePattern()
    {
        // variable declaration
        int difficulty;

        // choose difficulty
        if (numPlf <= 1)
        {
            difficulty = ChooseDifficulty(new float[] { 1f, 0f, 0f, 0f, 0f });
        }
        else if (numPlf <= 3)
        {
            difficulty = ChooseDifficulty(new float[] { 0.3f, 0.7f, 0f, 0f, 0f });
        }
        else if (numPlf <= 5)
        {
            difficulty = ChooseDifficulty(new float[] { 0.1f, 0.4f, 0.5f, 0f, 0f });
        }
        else if (numPlf <= 8)
        {
            difficulty = ChooseDifficulty(new float[] { 0f, 0.1f, 0.4f, 0.5f, 0f });
        }
        else if (numPlf <= 15)
        {
            difficulty = ChooseDifficulty(new float[] { 0f, 0f, 0.1f, 0.8f, 0.1f });
        }
        else
        {
            difficulty = ChooseDifficulty(new float[] { 0f, 0f, 0.0f, 0.5f, 0.5f });
        }

        int j = Random.Range(0, dif[difficulty].Length);
        int k = Random.Range(0, dif[difficulty][j].posPtn.Length);

        // return result
        return dif[difficulty][j].posPtn[k];
    }

    int ChooseDifficulty(float[] chance)
    {
        // variable declaration
        int numCasts = 0;
        int cast;

        // assign probability
        for (int i = 0; i < chance.Length; i++)
        {
            numCasts += (int)(100 * chance[i]);
        }

        // randomly choose difficulty
        cast = Random.Range(0, numCasts);

        // validate result
        for (int i = 0; i < chance.Length; i++)
        {
            cast -= (int)(100 * chance[i]);

            if (cast <= 0)
            {
                return i;
            }
        }

        return 0;
    }

    PLF_PTN InitPlfPattern(Vector2[] posPtn, float spawnRate, int[] sizeBias, float[] sizeRate)
    {
        // variable declaration
        PLF_PTN pattern = new PLF_PTN { };

        // assign values
        pattern.ptnIndex = ptnIndex++;
        pattern.posPtn = posPtn;
        pattern.sizePtn.bias = sizeBias;
        pattern.sizePtn.rate = sizeRate;

        // return result
        return pattern;
    }

    Vector2[] V2s(params Vector2[] pos)
    {
        return pos;
    }

    Vector2 V2(int x, int y)
    {
        return new Vector2(x, y);
    }

    int[] Int(params int[] i)
    {
        return i;
    }

    float[] Flt(params float[] f)
    {
        return f;
    }
}

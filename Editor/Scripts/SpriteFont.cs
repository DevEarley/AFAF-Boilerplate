using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFont : MonoBehaviour
{
    public Sprite space;
    public Sprite A;
    public Sprite B;
    public Sprite C;
    public Sprite D;
    public Sprite E;
    public Sprite F;
    public Sprite G;
    public Sprite H;
    public Sprite I;
    public Sprite J;
    public Sprite K;
    public Sprite L;
    public Sprite M;
    public Sprite N;
    public Sprite O;
    public Sprite P;
    public Sprite Q;
    public Sprite R;
    public Sprite S;
    public Sprite T;
    public Sprite U;
    public Sprite V;
    public Sprite W;
    public Sprite X;
    public Sprite Y;
	public Sprite Z;
	public Sprite a;
	public Sprite b;
	public Sprite c;
	public Sprite d;
	public Sprite e;
	public Sprite f;
	public Sprite g;
	public Sprite h;
	public Sprite i;
	public Sprite j;
	public Sprite k;
	public Sprite l;
	public Sprite m;
	public Sprite n;
	public Sprite o;
	public Sprite p;
	public Sprite q;
	public Sprite r;
	public Sprite s;
	public Sprite t;
	public Sprite u;
	public Sprite v;
	public Sprite w;
	public Sprite x;
	public Sprite y;
	public Sprite z;
	public Sprite hash;
	public Sprite dollar;
	public Sprite power;
	public Sprite openParen;
	public Sprite closeParen;
	public Sprite openCurley;
	public Sprite closeCurley;
	public Sprite openSquare;
	public Sprite closeSquare;
	public Sprite pipe;
	public Sprite semicolon;
	public Sprite colon;
	public Sprite quote;
	public Sprite doubleQuote;
	public Sprite lessThan;
	public Sprite greaterThan;
    public Sprite S1;
    public Sprite S2;
    public Sprite S3;
    public Sprite S4;
    public Sprite S5;
    public Sprite S6;
    public Sprite S7;
    public Sprite S8;
    public Sprite S9;
    public Sprite S0;
    public Sprite comma;
    public Sprite period;
    public Sprite bang;
    public Sprite at;
    public Sprite questionMark;
    public Sprite dash;
    public Sprite underscore;
    public Sprite plus;
    public Sprite times;
    public Sprite divide;
    public Sprite amp;
	public Sprite percent;
	public List<SpriteFontCustomCharacter>CustomCharacters = new List<SpriteFontCustomCharacter>();
	
	public float defaultCharacterSpacing = 0.2f;
 public Sprite GetSpriteForLetterOrCustomCharacter(string letter)
 {
 	var customCharacter = CustomCharacters.FirstOrDefault(c=>c.CharacterName==letter);
 	if(customCharacter == null)
	 	return GetSpriteForChar(letter[0]);
	 return customCharacter.Sprite;
 }
    public Sprite GetSpriteForChar(char letter)
    {
        switch (letter)
        {
        default:
        	case '?': return questionMark;
        	case '\n': return space;
        	case '\r': return space;
        	case ' ': return space;
  			case '\'': return quote;
  			case '"': return doubleQuote;
            case 'A': return A;
            case 'B': return B;
            case 'C': return C;
            case 'D': return D;
            case 'E': return E;
            case 'F': return F;
            case 'G': return G;
            case 'H': return H;
            case 'I': return I;
            case 'J': return J;
            case 'K': return K;
            case 'L': return L;
            case 'M': return M;
            case 'N': return N;
            case 'O': return O;
            case 'P': return P;
            case 'Q': return Q;
            case 'R': return R;
            case 'S': return S;
            case 'T': return T;
            case 'U': return U;
            case 'V': return V;
            case 'W': return W;
            case 'X': return X;
            case 'Y': return Y;
            case 'Z': return Z;
            case 'a': return a;
            case 'b': return b;
            case 'c': return c;
            case 'd': return d;
            case 'e': return e;
            case 'f': return f;
            case 'g': return g;
            case 'h': return h;
            case 'i': return i;
            case 'j': return j;
            case 'k': return k;
            case 'l': return l;
            case 'm': return m;
            case 'n': return n;
            case 'o': return o;
            case 'p': return p;
            case 'q': return q;
            case 'r': return r;
            case 's': return s;
            case 't': return t;
            case 'u': return u;
            case 'v': return v;
            case 'w': return w;
            case 'x': return x;
            case 'y': return y;
            case 'z': return z;
            case '0': return S0;
            case '1': return S1;
            case '2': return S2;
            case '3': return S3;
            case '4': return S4;
            case '5': return S5;
            case '6': return S6;
            case '7': return S7;
            case '8': return S8;
            case '9': return S9;
            case '!': return bang;
            case '@': return at;
            case '#': return hash;
            case '$': return dollar;
            case '^': return power;
            case '(': return openParen;
            case ')': return closeParen;
            case '{': return openCurley;
            case '}': return closeCurley;
            case '[': return openSquare;
            case ']': return closeSquare;
            case '|': return pipe;
            case ';': return semicolon;
            case ':': return colon;
            case '<': return lessThan;
            case '>': return greaterThan;
            case '.': return period;
            case ',': return comma;
            case '-': return dash;
            case '_': return underscore;
            case '*': return times;
            case '+': return plus;
            case '&': return amp;
            case '%': return percent;
            case '/': return divide;
        }
    }
}
[System.Serializable]
public class SpriteFontCustomCharacter
{
	public string CharacterName;
	public Sprite Sprite;
	public SpriteFontCustomCharacter(string characterName,Sprite sprite)
	{
		CharacterName = characterName;
		Sprite = sprite;
	}
}

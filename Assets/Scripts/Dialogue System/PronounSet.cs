using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PronounSet : ScriptableObject
{
    [SerializeField]
    private List<PronounGroup> pronouns;

    private static PronounGroup HeHim = new PronounGroup("he", "him", "his", "his", "himself");
    private static PronounGroup SheHer = new PronounGroup("she", "her", "her", "hers", "herself");
    private static PronounGroup TheyThem = new PronounGroup("they", "them", "their", "theirs", "themself");
    private static PronounGroup ItIts = new PronounGroup("it", "it", "its", "its", "itself");
    private static PronounGroup FaeFaer = new PronounGroup("fae", "faer", "faer", "faers", "faerself");
    private static PronounGroup ZeHir = new PronounGroup("ze", "hir", "hir", "hirs", "hirself");
    private static PronounGroup EyEm = new PronounGroup("ey", "em", "eir", "eirs", "emself");
    private static PronounGroup XeXir = new PronounGroup("xe", "xir", "xir", "xirs", "xirself");
    /// <summary>
    /// Adds the selected pronoun options to the list of pronouns.
    /// </summary>
    /// <param name="standardID"> A Bitmask representation of the prebuilt pronoun options. He, She, They, It, Fae, Ze, Ey, and Xe</param>
    public void AddPronouns(byte standardID)
    {
        if ((standardID & 1) != 0) //He/Him
        {
            pronouns.Add(HeHim);
        }
        if ((standardID & 2) != 0) //She/Her
        {
            pronouns.Add(SheHer);
        }
        if ((standardID & 4) != 0) //They/Them
        {
            pronouns.Add(TheyThem);
        }
        if ((standardID & 8) != 0) //It/Its
        {
            pronouns.Add(ItIts);
        }
        if ((standardID & 16) != 0) //Fae/Faer
        {
            pronouns.Add(FaeFaer);
        }
        if ((standardID & 32) != 0) //Ze/Hir
        {
            pronouns.Add(ZeHir);
        }
        if ((standardID & 64) != 0) //Ey/Em
        {
            pronouns.Add(EyEm);
        }
        if ((standardID & 128) != 0) //Xe/Xir
        {
            pronouns.Add(XeXir);
        }
    }

    public void AddPronouns(string i_subject, string i_dirObject, string i_depPossessive, string i_indPossessive, string i_reflexive)
    {
        pronouns.Add(new PronounGroup(i_subject, i_dirObject, i_depPossessive, i_indPossessive, i_reflexive));
    }

    public void ClearPronouns()
    {
        pronouns.Clear();
    }
    /// <summary>
    /// Get a pronoun in the proper conjugation for use in a sentence.
    /// </summary>
    /// <param name="tense">0 = subject, 1 = object, 2 = dependant, 3 = independant, 4 = reflexive</param>
    /// <returns></returns>
    public string GetPronoun(int tense)
    {
        PronounGroup pronoun = pronouns[Random.Range(0, pronouns.Count)];
        switch (tense)
        {
            case 0:
                return pronoun.subject;
            case 1:
                return pronoun.dirObject;
            case 2:
                return pronoun.depPossessive;
            case 3:
                return pronoun.indPossessive;
            case 4:
                return pronoun.reflexive;
            default:
                return "";
        }
    }

    private struct PronounGroup
    {
        public PronounGroup(string i_subject, string i_dirObject, string i_depPossessive, string i_indPossessive, string i_reflexive)
        {
            subject = i_subject;
            dirObject = i_dirObject;
            depPossessive = i_depPossessive;
            indPossessive = i_indPossessive;
            reflexive = i_reflexive;
        }
        
        public string subject, dirObject, depPossessive, indPossessive, reflexive;
    }

}

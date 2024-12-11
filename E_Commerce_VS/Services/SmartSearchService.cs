using F23.StringSimilarity;
using F23.StringSimilarity.Interfaces;
using System.Globalization;
using System.Text;

namespace E_Commerce_VS.Services;

public class SmartSearchService
{

    private const double THRESHOLD = 0.75;
    private static readonly string[] ITEMS = [
    "Alakazam EX Collection",
        "League Battle Deck: Regieleki & Miraidon",
        "Combined Powers: Lugia & Ho-Oh",
        "Lucario VStar Premium Collection",
        "Sword & Shield: Astral Radiance Pack",
        "Sprigatito Paldea Collection",
        "Carpeta Plástico GuardaCartas",
        "Maletin GuardaCartas Charizard Theme",
        "Pokemon Card Game Case",
        "Sword & Shield: Lost Origin Pack",
        "Pikachu V Collection",
        "Paradox Powers EX: Koraidon",
        "Sword & Shield: Silver Tempest (Togetic)",
        "Baraja Combate EX: Iron Leaves & Tapu Koko",
        "Pokemon TCG Arceus Case",
        "Escarlata y Purpura: Mascara Crepuscular",
        "Sword & Shield: Silver Tempest",
        "Escarlata y Purpura: Llamas Obsidianas",
        "Escarlata y Purpura: Evoluciones En Paldea",
        "Sword & Shield: Evolving Skies"
    ];

    private readonly INormalizedStringSimilarity _stringSimilarityComparer;

    public SmartSearchService()
    {
        _stringSimilarityComparer = new JaroWinkler();
    }

    public IEnumerable<string> Search(string query)
    {
        IEnumerable<string> result;

        if (string.IsNullOrWhiteSpace(query))
        {
            result = ITEMS;
        }

        else
        {
            string[] queryKeys = GetKeys(ClearText(query));
            List<string> matches = new List<string>();

            foreach (string item in ITEMS)
            {
                string[] itemKeys = GetKeys(ClearText(item));

                if (IsMatch(queryKeys, itemKeys))
                {
                    matches.Add(item);
                }
            }

            result = matches;
        }

        return result;
    }

    private bool IsMatch(string[] queryKeys, string[] itemKeys)
    {
        bool isMatch = false;

        for (int i = 0; !isMatch && i < itemKeys.Length; i++)
        {
            string itemKey = itemKeys[i];

            for (int j = 0; !isMatch && j < queryKeys.Length; j++)
            {
                string queryKey = queryKeys[j];

                isMatch = IsMatch(itemKey, queryKey);
            }
        }

        return isMatch;
    }

    private bool IsMatch(string itemKey, string queryKey)
    {
        return itemKey == queryKey
            || itemKey.Contains(queryKey)
            || _stringSimilarityComparer.Similarity(itemKey, queryKey) >= THRESHOLD;
    }

    private string[] GetKeys(string query)
    {
        return query.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    }

    private string ClearText(string text)
    {
        return RemoveDiacritics(text.ToLower());
    }

    private string RemoveDiacritics(string text)
    {
        string normalizedString = text.Normalize(NormalizationForm.FormD);
        StringBuilder stringBuilder = new StringBuilder(normalizedString.Length);

        for (int i = 0; i < normalizedString.Length; i++)
        {
            char c = normalizedString[i];
            UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }

        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }
}
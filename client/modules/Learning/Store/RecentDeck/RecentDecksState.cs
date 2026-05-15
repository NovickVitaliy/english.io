using Fluxor;

namespace Learning.Store.RecentDeck;

public record RecentDeckEntry(Guid Id, string Name, DateTime AccessedAt);

[FeatureState]
public record RecentDecksState
{
    public List<RecentDeckEntry> RecentDecks { get; init; } = new();
}

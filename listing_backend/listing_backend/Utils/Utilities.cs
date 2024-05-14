namespace listing_backend.Utils;

public static class Utilities
{
    public static void UpdateCollection<T>(ICollection<T> existingItems, ICollection<T> newItems) where T : class
    {
        if (existingItems.Equals(newItems))
        {
            return;
        }
        
        foreach (var existingItem in existingItems.ToList().Where(existingItem => !newItems.Contains(existingItem)))
        {
            existingItems.Remove(existingItem);
        }
        
        foreach (var newItem in newItems.ToList().Where(newItem => !existingItems.Contains(newItem)))
        {
            existingItems.Add(newItem);
        }
    }
}
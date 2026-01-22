# Exploration Log

## 1. Buff Memory Structure Analysis (Raw Data Exposure)

### Hypothesis
The game client stores active buffs in an array at `statusBufferAddress`. We hypothesized that each buff entry might consist of a structure larger than a single 32-bit integer (the ID). Specifically, we suspected a structure like:
-   Offset 0: Buff ID
-   Offset 4: Value 1 (e.g., Duration, Level, Amount)
-   Offset 8: Value 2
-   Offset 12: Value 3

This hypothesis was based on standard game memory structures where effects often carry associated data.

### Experiment
We implemented a feature in the **Dev Tab** to read and display the 3 integers immediately following each detected Buff ID (i.e., at offsets +4, +8, and +12 bytes from the ID address).

### Results
-   **Observation**: The exposed raw values did not provide any recognizable or useful patterns. They did not correlate meaningfully with buff duration, level, or other visible stats.
-   **Conclusion**: The `statusBufferAddress` likely points to a simple array of IDs (flat list), or the associated data is stored in a completely separate parallel structure/address space not adjacent to the ID array.
-   **Action**: The "Raw Data Exposure" feature (columns `Raw +4`, `Raw +8`, `Raw +12`) was removed from the codebase to keep the UI clean.

### Reference Code (Removed)
The following method was temporarily added to `Model/Client.cs` and subsequently removed:

```csharp
public List<uint> GetBuffRawData(int effectStatusIndex)
{
    List<uint> values = new List<uint>();
    int baseAddress = this.statusBufferAddress + effectStatusIndex * 4;
    // Reads ID + next 3 integers
    for (int i = 0; i < 4; i++)
    {
        values.Add(ReadMemory(baseAddress + (i * 4)));
    }
    return values;
}
```

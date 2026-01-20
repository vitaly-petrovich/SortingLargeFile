### Test Assignment
Your task is to process a **large text file** where each line follows the format: 

`<Number>. <String>`

### Example Input:
```
415. Apple
30432. Something something something
1. Apple
32. Cherry is the best
2. Banana is yellow
```

### Sorting Criteria:
1. First, sort by the string part (alphabetically).
2. If two lines have the same string, sort by the number (ascending).

### Expected Output:
```
1. Apple
415. Apple
2. Banana is yellow
32. Cherry is the best
30432. Something something something
```
### Implementation Requirements
You must develop **two C# programs**:
1. Test File Generator
    - Creates a text file of the described format.
    - There must be some number of lines with the same String part.
    - Allows specifying the file size.
2. Sorter
    - Sorts the file according to the given criteria.
    - Must handle very large files (~100GB) efficiently.

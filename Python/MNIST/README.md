# Step-by-step introduction to the MNIST handwritten digits recognition example

This is for learning purposes only.

The MINST dataset contains 70000 greyscale images of handwritten digits from 0 to 9.
Each image is 28 pixels by 28 pixels.
MNIST is often referred to as the "Hello World" of machine learning, so is an ideal place to start.

While the solution is only a few lines of code, when you're unfamiliar with the process, this can seem like magic.
Each cell slowly breaks down the example, showing individual aspects, and introducing new concepts.

This is also helpful for when you're not that familiar with Python!

This is adapted from the explanation given by Nick Ovchinnikov:  
Dive Into Learning From Data: MNIST with Logistic Regression  
https://www.udemy.com/course/dive-into-learning-from-data

# Getting Started

```shell
cd Python\MNIST

python -m venv .venv

.venv\Scripts\activate

pip install -r requirements.txt
```

**Execution notes:**

-   Unless otherwise specified, cells must be run in sequence
-   Many cells rely on previous cells having executed, and data variables set globally
-   Rerunning an earlier cell, after having run a cell that comes after it, may produce incorrect results if global
    variables have changed in the mean time
-   All functions are self-contained, i.e. they must be passed values, even if those values are globally available
-   Many cells use functions specific to that cell to avoid intermediate variables polluting the global namespce

**Disclaimer:**

All notes and code are provided in good faith. However, because this is a learning execise for me, I may
have misinterpreted information. Although every effort has been made to be as accurate as possible,
this is a first parse, and is subject to change, or correction, as my understanding grows.

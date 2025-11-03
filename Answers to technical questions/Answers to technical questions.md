# 📌 Answers to Technical Questions

**🔸 1. How much time did you spend on this task?**  
I spent around 16 hours on this task, including setting up the project, understanding the requirements, and implementing basic features.

# 

**🔸 2. If you had more time, what improvements or additions would you make?**  
If I had more time, I would go through different parts of the project carefully, looking for any issues or features that could be improved.  
For example, if I noticed something in the database causing slow queries, I would clean up and optimize those queries.  
If I saw parts of the code that could be clearer or more maintainable, I would refactor them following clean code principles.  
I would also check for any potential performance bottlenecks, think about caching strategies, and see where asynchronous operations or background jobs could make the API more responsive.  
Basically, I would explore the project, identify areas that could be stronger or cleaner, and apply improvements to make it more robust and easier to maintain.

# 

**🔸 3. What is the most useful feature recently added to your favorite programming language? Please include a code snippet to demonstrate how you use it.**  
I like Python 3.13 (the free-threaded version without GIL). It's a big improvement because it allows true multi-threading, which can significantly speed up CPU-bound tasks.  

```python
import threading

def compute_heavy(x):
    return sum(i*i for i in range(x))

threads = [threading.Thread(target=compute_heavy, args=(1000000,)) for _ in range(4)]
[t.start() for t in threads]
[t.join() for t in threads]

print("Done computing in parallel!")
```

# 

**🔸 4. How do you identify and diagnose a performance issue in a production environment? Have you done this before?**

I usually start by identifying which part of the software is the bottleneck — the section that slows down everything else. I use profiling tools or logging to pinpoint the slow functions. Once identified, I optimize algorithms, caching, or database queries. Yes, I have practiced this approach in small projects.

# 

**🔸 5. What’s the last technical book you read or technical conference you attended? What did you learn from it?**

I read *Clean Code*. It’s a small, practical book. Key takeaways:

- Write readable and maintainable code.
- Functions should do one thing and be small.
- Naming matters a lot.
- Avoid duplication.
- Refactoring is essential for long-term code health.

# 

**🔸 6. What’s your opinion about this technical test?**

This technical test was very effective. It challenged my programming knowledge and problem-solving skills, allowing me to demonstrate both understanding and creativity. To enhance the task that was provided, I implemented several additional features on my own.


# 

**🔸 7. Please describe yourself using JSON format.**

```json
{
  "fullname": "Behnoush Shahraeini",
  "role": "Backend Developer",
  "skills": [
    "Python", "Django", "Django REST Framework","C#", ".NET",
    "Docker", "Redis", "Celery", "Git", "GitHub", "SQL",
    "PostgreSQL", "HTML", "CSS"
  ],
  "experience": [
    {
      "title": "Backend Developer (Django)",
      "type": "Remote",
      "period": "2023 – 2025",
      "highlights": [
        "Designed and built RESTful APIs using Django & DRF",
        "Implemented authentication (JWT, OTP)",
        "Automated tasks with Celery + Redis",
        "Optimized DB queries and used caching for performance",
        "Containerized apps with Docker",
        "Worked directly with clients in freelance projects"
      ]
    },
    {
      "title": "Senior Technical Support",
      "company": "Saman Solutions (Mobillet)",
      "period": "2023 – 2024"
    }
  ],
  "projects": {
    "python_django": [
      "Charity Project (Django REST API)",
      "TodoFlow (Django + Celery + OTP Login)"
    ],
    "csharp_practice": [
      "Console Apps practicing OOP, exception andling, and modular code",
      "File-based data processing and calculations",
      "Hands-on debugging and writing clean code"
    ]
  },
  "certificates": [
    "Back-End Development with Django – Quera",
    "Algorithm Level-Up – Quera"
  ],
  "github": "https://github.com/Behnoushin"
}

```
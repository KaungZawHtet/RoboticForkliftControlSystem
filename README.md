# Robotic Forklift Control System

You can check this app at [forklift.kaungzawhtet.site](https://forklift.kaungzawhtet.site/)

## Engineering Thoughts

Here are the notable thoughts I got on the craft. I suggest to read this before moving to new

### mono repo vs multi repo

In my previous projects, we used multi repo a lot (eg. separate repos for FE and BE or one repo for one microservices) but these days, I wanted to give a shot to the mono repo solution. But in the daily basic, I will follow team's preference.


### Maintainability 

I tried to improve the maintainability and best practices in BE by doing these things

    1. Leveraging modern language features (eg. preferred, but not limited to, pattern matching, switch expressions, records, file-scoped namespaces  )
    2. Proper naming for variables
    3. Used common N-tiered architecture just like my previous projects and design patterns like DTO,
    4. Magic string or number management
    5. Proper Err Handling with try-catch at controllers (alternative / enhanced solution I thought was err handling middleware but I decided to skip for now)
    4. Considered common principles like DRY, KISS, YAGNI, Separation of Concerns and SOLID
    5. Unit Tests

For React, I made sure these things
    1. Used proper and clean folder structure
    2. Used proper Linter and formatter usage
    3. Considered component reusability


### Room for improvement

When Project is not big enough, I skip these features

1. More complete App Insight sinks
2. Testing at FE (eg. playwright or vite testing)
3. Magic String management in FE because according to my previous experience, proper string/Text management in FE is about ability to handle i18n. (Eg. keep the strings in json and manage with i18n Lib ). For now, we don't have i18n requirement so I considered to skip it.
4. delete feature in forklift table would give smoother experience. 

## Notes & Assumptions
    
Duplicate model numbers are allowed since requirements didn’t specify uniqueness. 
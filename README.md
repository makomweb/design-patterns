# Design Patterns

1) Creational
2) Structural
3) Behavioral

## SOLID

- single responsibility principle
    - a class hould only have one reason to change
    - _separation of concerns_ - different classes handling different, independent tasks/problems

- open-closed principle
    - classes should be open for extension and closed for modification

- liskov-substituation principle
    - you should be able to subsitute a base type for a subtype

- interface segregation principle
    - don't put too much into an interface
    - split it into separate interfaces
    - YAGNI - you ain't going to need it

- dependency inversion principle
    - high-level modules should not depend upon low-level ones
    - use abstraction
# Creational patterns
## Builder pattern

- a builder is a separate component for building an object
- can either give builder a constructor or return it via a static function
- to make builder fluent, return _this_
- different facets of an object can be built with different builders working in tandem via a base class
## Factory pattern

- a factory method is a static method that creates objects
- a factory can take care of object creation
- a factory can be external or reside inside the class a an inner class
- hierarchies of factories can be used to create related objects
## Prototype pattern

- to implement a prototype, partially construct an object and store it somewhere
- clone the prototype
    - implement your own deep copy functionality, or
    - serialize and deserialize
- customized the resulting instance
## Singleton and MonoState pattern

- making a *safe* singleton is easy:
    - construct a static `Lazy<T>` and return its _value_
- singletons are difficult to test
- instead of directly using a singleton, consider depending on an abstraction (e.g., an interface)
- consier defining singleton lifetime in DI container

# Structural patterns
## Adapter pattern

- implementing an adapter is easy
- determine the API you have and the API you need
- create a compount which aggregates (has a reference to, ...) the adaptee
- intermediate representations can pile up: use caching and other optimizations
## Bridge pattern

- decouple abstraction from implementation
- both can exist as hierarchies
- a stronger form of encapsulation

## Composite pattern

- objects can use other objects via inheritance/composition
- some composed and singular objects need similar/identical behaviors
- composite design pattern lets us treat both types of objects uniformly
- C# has special support for the _enumeration_ concept
- a single object can masquerade as a collection with `yield return this;`

## Chain Of Responsibility pattern

```
A chain of components who all get a chance to process a command or a query, optionally having default processing implementation and an ability to terminate the processing chain.
```

### Command Query Separation

- command = asking for an action or change (e.g., please set your attack value to 2)
- query = asking for information (e.g., please give me your attack value)
- _CQS_ = having separate means of sending commands and queries to e.g., direct field access

### Chain of responsibility: summary

- can be implemented as a chain of references or a centralized construct
- enlist objects in the chain, possibly controlling their order
- object removal from chain (e.g., in `Dispose()`)


## Proxy

### What's the difference between decorator and proxy?

![image](./Images/summary-proxy-vs-decorator)

## Chain Of Responsibility pattern

![image](./Images/introduction-chain-of-responsibility.png)
![image](./Images/introduction-cqs.png)
![image](./Images/summary-chain-of-responsibility.png)

## Command pattern

![image](./Images/summary-command.png)

## Interpreter pattern


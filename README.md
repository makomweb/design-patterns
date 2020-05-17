# SOLID

- single responsibility principle
    - a class should only have one reason to change
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

# Design Patterns

1) Creational
2) Structural
3) Behavioral

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

## Decorator pattern

- a decorator keeps the reference to the decorated object(s)
- may or may not proxy over calls
    - use _Resharper_ to generate deleget members
- exists in a _static_ variation
    - `X<Y<Foo>>`
    - very limited due to inability to inherit from type parameters
    
## Facade pattern

- build a facade to provide a simplified API over a set of classes
- may wish to (optionally) expose internals through the facade
- may allow users to 'escalate' to use more complex APIs if they need to

## Flyweight

- store common data externally
- define the idea of 'ranges' on homogeneous collections and store data related to those ranges
- .NET string interning _is_ the _Flyweight_ pattern

## Proxy

### What's the difference between decorator and proxy?

- proxy provides an identical interface;
decorator provides an enhanced interface
- decorator typically aggregates (or has reference to) what it is decorating;proxy does not have to
- proxy might not even be working with a materialized object

### Poxy: summary

- a proxy has the same interface as the underlying object
- to create a proxy, simply replicate the existing interface of an object
- add relevant functionality to the redefined member functions
- different proxies (communication, logging, caching) have completely different behaviors

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

## Command pattern

- encapsulate all details of an object operation in a separate object
- define instruction for applying the command (either in the command itself, or elsewhere)
- optionally define instructions for undoing the command
- can create composite commands (a.k.a. macros)

## Interpreter pattern

[www.antlr.org](http://www.antlr.org/tools.html)

Existing lexers and parsers for various programming languages.

### Interpreter pattern: Summary

- barring simple cases, an interpreter acts in two stages
    1) lexing turns input into a set of tokens, e.g. 
    `3*(4+5)` becomes `Lit[3] Star LeftParen Lit[4] Plus Lit[5] RightParen`
    2) parsing turns tokens into meaningful constructs, e.g.
    ```
    MultiplicationExpression[
        Integer[3],
        AdditionExpression[
            Integer[4], Integer[5]
        ]
    ]
    ```
- parsed data can be traversed

## Iterator pattern

- an iterator specifies how you can traverse an object
- an iterator object, unlike a method, cannot be recursive
- generally, an `IEnumerable<T>` -returning method is enough
- iteration works through _duck typing_ - you need a `GetEnumerator()` that yields a type that has `Current` and `MoveNext()`
- traversing a tree can be done _inorder_, _preorder_, _postorder

## Mediator pattern

- create the mediator and have each object in the system refer to it
    - e.g., in a field
- mediator engages in bidirectional communication with it's connected components
- mediator has functions the component can call
- components have functions the mediator can call
- event processing (e.g., Rx) libraries make communication easier to implement
- `MediatR` is a powerful mediator NuGET package
 
## Memento pattern

- mementos are used to roll back state arbitrarily
- a memento is simpply a token/handle class with (typically) no functions of its own
- a memento is not required to expose directly the state(s) to which it reverts the system
- can be used to implement undo/redo

## Null Object pattern

- implement the required interface
- rewrite the methods with empty bodies
    - if method is non-void, return `default(T)`
    - if these values are ever used, you are in trouble 
- supply an instance of Null Object in place of actual object
- dynamic construction possible 
    - with associated performance implications

## Observer pattern

- observer is an intrusive approach: an observable must provide an event to subscribe to
- special care must be taken to prevent issues in multithreaded scenarios
- .NET comes with observable collections
- `IObserver<T>/IObservable<T>` are used in stream processing (Reactive Extensions)

## State pattern

- define possible states and events/triggers
- can define
    - state entry/exit behaviors
    - action when a particular event causes a transition
    - guard conditions enabling/disabling a transition
    - default action when no transition is found for an event

## TO BE IMPLEMENTED

- Strategy
- Template Method
- Visitor

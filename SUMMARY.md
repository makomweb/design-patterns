
# Course Summary

## Creational patterns

- builder
    - separate component for when object construction get too complicated
    - can create mutually cooperating sub-builders
    - often has a fluent interface

- factories
    - method more expressive than constructor
    - can be an outside class or inner class which has the benefit of accessing private members

- prototype
    - creation of object from an existing object
    - requirese either explicit deep copy

- singleton
    - when you need to ensure there is only one instance
    - made thread-safe and lazy with `Lazy<T>`
    - consider extracting interface or using dependency injection

## Structural patterns

- adapter
    - convert an interface you get to the interface you need
- bridge
    - decouple abstraction from implementation
- composite
    - allows clients to treat individual objects and compositions of objects uniformly
- decorator
    - attach additional responisibilities to objects
- facade
    - provide a single unified interface over a set of classes / systems
- flyweight
    - efficiently support very large numbers of similar objects
- proxy
    - provide a surrogate object that forwards calls to the real object while performing additional functions
    - e.g., access control, communication, logging, etc.
    - dynamic proxy - without the necessity of replicating a target object API

## Behavioral patterns

- chain of responsibility
    - allow component to process information/events in a chain
    - each element in the chain refers to next element; or
    - make a list and go through it

- command 
    - encapsulate a request into a separate object
    - good for audit, replay, undo/redo
    - part of CQS/CQRS (query is also, effectively, a command)

- interpreter
    - transform textual input into object-oriented structures
    - used by interpreters, compilers, static analysis tools, etc.
    - _Compiler Theory_ is a separate branch of Computer Science

- iterator
    - provides an interface for accessing elements of an aggregate object
    - `IEnumerable<T>` should be used in 99% of cases

- mediator
    - provides mediation services between objects
    - e.g., message passing, chat room

- memento
    - yields token representing system states
    - tokens do not allow direct manipulation, but can be used in appropriate APIs

- observer
    - built into C# with the _event_ keyworkd
    - additional support provided for properties, collections and observable streams

- state
    - we model systems by having one of a possible states and transitions between these states
    - such a system is called _state machine_
    - special frameworks exist to orchastrate state machines

- strategy & template method
    - both patterns define an algorithm blueprint/placeholder
    - strategy uses composition, template method uses inheritance

- visitor
    - addition functionality to existing classes through double dispatch
    - dynamic visitor possible, but with performance costs
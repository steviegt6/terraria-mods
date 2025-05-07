DAYSHADE introduces new APIs for loading that fundamentally change how loading
functions if developers so choose to use the features.

Instead of pushing for ModX implementations and weird fragmentation between
singleton and entity-specific data, it treats all content implementations as
singletons and provides extra APIs for managing instanced data in a manner
befitting the expected use cases (e.g. APIs for ResetEffects in Players).

Furthermore, hooks are made generic when applicable and can be included anywhere
akin to loadable APIs.

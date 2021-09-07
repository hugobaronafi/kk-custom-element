<div>
  <slot />
</div>

<style>
  div :global(.group) {
    display: flex;
    margin-bottom: 0.5em;
  }

  div :global(.column) {
    flex-direction: column;
  }

  div :global(.item) {
    flex: 1;
  }

  @media (max-width: 800px) {
    div :global(.group) {
      flex-direction: column;
    }
  }
</style>

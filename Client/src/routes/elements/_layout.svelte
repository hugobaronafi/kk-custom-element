<div>
  <slot />
</div>

<style>
  :global(main) {
    background: none;
  }

  div :global(.group) {
    display: flex;
    margin-bottom: 0.5em;
  }

  div :global(.wrap) {
    flex-wrap: wrap;
  }

  div :global(.column) {
    flex-direction: column;
  }

  div :global(.item) {
    flex: 1;
  }

  div :global(.label) {
    font-size: 0.8em;
    font-weight: 600;
    color: #919194;
    margin-bottom: 0.5em;
  }

  div :global(.input) {
    border: none;
    outline: none;
    font-size: 1em;
    font-family: Source Sans Pro, sans-serif;
    width: 100%;
  }

  div :global(.input:focus) {
    border-color: #0a68f5 !important;
    outline: none;
  }

  div :global(.select) {
    border: none;
    font-family: inherit;
    font-size: inherit;
    line-height: inherit;
    margin: 0.4em 0em;
  }

  div :global(.select:focus) {
    outline: none;
  }

  div :global(.button) {
    font: inherit;
    display: inline-block;
    cursor: pointer;
    border-style: solid;
    transition: all 0.25s cubic-bezier(0.23, 1, 0.32, 1) 50ms, border-width 0ms,
      padding 0ms;
    color: #4c4d52;
    background-color: initial;
    border-color: #919194;
    padding: 0.25em 0.8em;
    min-width: 48px;
    margin: 0.6em 0.6em 0.4em;
    font-size: 0.85em;
    border-width: 0.1em;
    border-radius: 100em;
    text-transform: uppercase;
    color: rgb(55, 81, 124);
    font-weight: 600;
  }

  div :global(.button.destructive:focus, .button.destructive:hover) {
    color: #fff !important;
    background-color: #f02222 !important;
    border-color: transparent !important;
  }

  div :global(.button:focus, .button:hover) {
    background-color: rgb(175, 197, 233) !important;
    border-color: rgb(111, 111, 111) !important;
  }

  @font-face {
    font-family: kentico-icons;
    font-style: normal;
    font-weight: 400;
    src: url("/kentico-icons-v1.6.0.woff") format("woff");
  }

  :global(.icon) {
    display: inline-block;
    font-family: kentico-icons;
    -webkit-font-smoothing: antialiased;
    -moz-osx-font-smoothing: grayscale;
    font-style: normal;
    font-weight: 400;
    font-variant: normal;
    text-transform: none;
    padding: 0.6em 0.8em;
    font-size: 1em;
    color: #424242;
    cursor: pointer;
  }

  :global(.icon.delete:before) {
    content: "\e6d0";
  }

  :global(.delete:hover) {
    color: #fff;
    background-color: #f02222;
  }

  :global(.icon.download:before) {
    content: "\E6DD";
  }

  :global(.download:hover) {
    color: #fff;
    background-color: #0a68f5;
  }

  :global(.icon.remove:before) {
    content: "\E60E";
  }

  :global(.remove:hover) {
    color: #fff;
    background-color: #f02222;
  }

  :global(.arrow-right-top-square:before) {
    content: "\e6d8";
  }

  :global(.rectangle-a:before) {
    content: "\e61e";
  }

  :global(.braces:before) {
    content: "\e6cb";
  }

  :global(.octothorpe:before) {
    content: "\e904";
  }

  :global(.calendar:before) {
    content: "\e6b9";
  }

  @media (max-width: 800px) {
    div :global(.group) {
      flex-direction: column;
    }
  }
</style>

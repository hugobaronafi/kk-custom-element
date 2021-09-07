<script lang="ts">
  import hljs from "highlight.js/lib/core";
  import json from "highlight.js/lib/languages/json";
  import "highlight.js/styles/stackoverflow-light.css";

  export let code: string;

  hljs.registerLanguage("json", json);
</script>

<code>{@html hljs.highlight('json', code, true).value}</code>

<style>
  code {
    font-family: menlo, inconsolata, monospace;
    font-size: calc(1em - 2px);
    color: #555;
    background-color: #f0f0f0;
    padding: 0.2em 0.4em;
    border-radius: 2px;
    display: block;
    white-space: pre-wrap;
    max-height: 20em;
    overflow-y: auto;
    word-break: break-all;
    border-radius: 0.3em;
  }
</style>

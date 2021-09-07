<script lang="ts">
  import tinycolor from "tinycolor2";
  import { round } from "lodash";
  import { fade } from "svelte/transition";

  import Invalid from "../../../shared/components/customElement/invalid.svelte";
  import CustomElement from "../../../shared/components/customElement/customElement.svelte";
  import { translate } from "../../../shared/stores/translate";
  import Loading from "../../../shared/components/loading.svelte";
  import sharedTranslations from "../../../shared/components/customElement/resources";
  import translations from "./_resources";

  interface IColorConfig {
    presets: string[];
  }

  interface IColorValue {
    hue: number;
    saturation: number;
    lightness: number;
  }

  let value: IColorValue = { hue: 360, saturation: 50, lightness: 50 };
  let config: IColorConfig;
  let disabled: boolean;

  let tinyColor: {
    toHsl: () => { h: number; s: number; l: number };
    toRgb: () => { r: number; g: number; b: number };
    toHex: () => string;
  };

  let hue: number;
  let saturation: number;
  let lightness: number;
  let red: number;
  let green: number;
  let blue: number;
  let hexValue: string;

  let container: HTMLDivElement;
  let dragging = false;

  $: if (tinyColor) {
    const { h, s, l } = tinyColor.toHsl();
    const { r, g, b } = tinyColor.toRgb();

    hue = round(h);
    saturation = round(s * 100);
    lightness = round(l * 100);
    red = r;
    green = g;
    blue = b;
    hexValue = tinyColor.toHex();

    value = {
      hue,
      saturation,
      lightness,
    };
  }

  $: if (container) {
    container.style.setProperty("--hue", `${hue}`);
    container.style.setProperty("--saturation", `${saturation}%`);
    container.style.setProperty("--lightness", `${lightness}%`);
  }

  const click: svelte.JSX.MouseEventHandler<HTMLDivElement> = (event) => {
    if (!disabled) {
      tinyColor = tinycolor({
        h: hue,
        s: event.offsetX / event.currentTarget.scrollWidth,
        l: 1 - event.offsetY / event.currentTarget.scrollHeight,
      });
    }
  };

  const t = translate([translations, sharedTranslations]);
</script>

<CustomElement
  bind:value
  bind:config
  bind:disabled
  on:ready={() =>
    (tinyColor = tinycolor({
      h: value.hue,
      s: `${value.saturation}%`,
      l: `${value.lightness}%`,
    }))}>
  <div class="container" bind:this={container} transition:fade>
    <div class="visuals item">
      <div
        class="box item"
        on:mousedown={() => (dragging = true)}
        on:mousemove={(event) => dragging && click(event)}
        on:mouseup={() => (dragging = false)}
        on:click={click}>
        <div class="hue overlay" />
        <div class="saturation overlay" />
        <svg class="pointer" height="10" width="10">
          <line x1="5" x2="5" y1="0" y2="10" stroke-width="2" stroke="white" />
          <line x1="0" x2="10" y1="5" y2="5" stroke-width="2" stroke="white" />
        </svg>
      </div>
      <input
        class="slider item"
        type="range"
        min="0"
        max="360"
        {disabled}
        value={hue}
        on:input={(event) =>
          (tinyColor = tinycolor({
            h: event.currentTarget.value,
            s: `${saturation}%`,
            l: `${lightness}%`,
          }))} />
    </div>
    <div class="inputs item">
      <div class="group">
        <label class="group column item"
          ><div class="label">{$t("hue")}</div>
          <input
            class="input"
            type="number"
            min="0"
            max="360"
            {disabled}
            value={hue}
            on:input={(event) =>
              (tinyColor = tinycolor({
                h: event.currentTarget.value,
                s: `${saturation}%`,
                l: `${lightness}%`,
              }))} />
        </label>
        <label class="group column item"
          ><div class="label">
            {$t("saturation")}
          </div>
          <input
            class="input"
            type="number"
            min="0"
            max="100"
            {disabled}
            value={saturation}
            on:input={(event) =>
              (tinyColor = tinycolor({
                h: hue,
                s: `${event.currentTarget.value}%`,
                l: `${lightness}%`,
              }))} />
        </label>
        <label class="group column item">
          <div class="label">{$t("lightness")}</div>
          <input
            class="input"
            type="number"
            min="0"
            max="100"
            {disabled}
            value={lightness}
            on:input={(event) =>
              (tinyColor = tinycolor({
                h: hue,
                s: `${saturation}%`,
                l: `${event.currentTarget.value}%`,
              }))} />
        </label>
      </div>
      <div class="group">
        <label class="group column item"
          ><div class="label">{$t("red")}</div>
          <input
            class="input"
            type="number"
            min="0"
            max="255"
            {disabled}
            value={red}
            on:input={(event) =>
              (tinyColor = tinycolor({
                r: event.currentTarget.value,
                g: green,
                b: blue,
              }))} />
        </label>
        <label class="group column item"
          ><div class="label">{$t("green")}</div>
          <input
            class="input"
            type="number"
            min="0"
            max="255"
            {disabled}
            value={green}
            on:input={(event) =>
              (tinyColor = tinycolor({
                r: red,
                g: event.currentTarget.value,
                b: blue,
              }))} />
        </label>
        <label class="group column item">
          <div class="label">{$t("blue")}</div>
          <input
            class="input"
            type="number"
            min="0"
            max="255"
            {disabled}
            value={blue}
            on:input={(event) =>
              (tinyColor = tinycolor({
                r: red,
                g: green,
                b: event.currentTarget.value,
              }))} />
        </label>
      </div>
      <div class="group">
        <label class="group column item"
          ><div class="label">{$t("hex")}</div>
          <input class="input" disabled bind:value={hexValue} />
        </label>
        <div class="group item" />
        <div class="group item" />
      </div>
      <div class="group">
        <div class="preview" />
      </div>
      <div class="item" />
      {#if !disabled && config.presets && config.presets.some((preset) =>
          tinycolor(preset).isValid()
        )}
        <div class="group column">
          <div class="label">{$t("presets")}</div>
          <div class="presets">
            {#each config.presets.filter((preset) =>
              tinycolor(preset).isValid()
            ) as preset}
              <div
                class="preset"
                style={`background: ${preset}`}
                on:click={() => (tinyColor = tinycolor(preset))} />
            {/each}
          </div>
        </div>
      {/if}
    </div>
  </div>
  <div slot="loading">
    <Loading />
  </div>
  <div slot="invalid">
    <Invalid />
  </div>
</CustomElement>

<style>
  .container {
    display: flex;
    height: 20em;
  }

  .visuals {
    display: flex;
    flex-direction: column;
  }

  .box {
    display: flex;
    position: relative;
    flex: 10;
  }

  .hue {
    background: hsl(var(--hue), 100%, 50%);
  }

  .saturation {
    background: -moz-linear-gradient(
        top,
        hsl(0, 0%, 100%) 0%,
        hsla(0, 0%, 100%, 0) 50%,
        hsla(0, 0%, 0%, 0) 50%,
        hsl(0, 0%, 0%) 100%
      ),
      -moz-linear-gradient(left, hsl(0, 0%, 50%) 0%, hsla(0, 0%, 50%, 0) 100%);
    background: -webkit-linear-gradient(
        top,
        hsl(0, 0%, 100%) 0%,
        hsla(0, 0%, 100%, 0) 50%,
        hsla(0, 0%, 0%, 0) 50%,
        hsl(0, 0%, 0%) 100%
      ),
      -webkit-linear-gradient(left, hsl(0, 0%, 50%) 0%, hsla(0, 0%, 50%, 0) 100%);
    background: -ms-linear-gradient(
        top,
        hsl(0, 0%, 100%) 0%,
        hsla(0, 0%, 100%, 0) 50%,
        hsla(0, 0%, 0%, 0) 50%,
        hsl(0, 0%, 0%) 100%
      ),
      -ms-linear-gradient(left, hsl(0, 0%, 50%) 0%, hsla(0, 0%, 50%, 0) 100%);
    background: -o-linear-gradient(
        top,
        hsl(0, 0%, 100%) 0%,
        hsla(0, 0%, 100%, 0) 50%,
        hsla(0, 0%, 0%, 0) 50%,
        hsl(0, 0%, 0%) 100%
      ),
      -o-linear-gradient(left, hsl(0, 0%, 50%) 0%, hsla(0, 0%, 50%, 0) 100%);
  }

  .overlay {
    position: absolute;
    height: 100%;
    width: 100%;
  }

  .pointer {
    position: absolute;
    left: var(--saturation);
    top: calc(100% - var(--lightness));
    transform: translate(-50%, -50%);
    pointer-events: none;
  }

  .pointer:before {
    content: "+";
    display: flex;
    flex-direction: row;
    align-items: center;
    justify-content: center;
    color: white;
    line-height: 0;
    font-size: 2em;
  }

  .slider {
    appearance: none;
    background: linear-gradient(
      to right,
      #ff0000 0%,
      #ffff00 calc((100% * 1) / 6),
      #00ff00 calc((100% * 2) / 6),
      #00ffff calc((100% * 3) / 6),
      #0000ff calc((100% * 4) / 6),
      #ff00ff calc((100% * 5) / 6),
      #ff0000 100%
    );
    border-radius: 0.2em;
    width: 100%;
    margin: 0;
    outline: none;
    margin-top: 0.5em;
  }

  .slider::-webkit-slider-thumb {
    -webkit-appearance: none;
    background: white;
    height: 2em;
    width: 0.2em;
    border-radius: 5px;
    cursor: pointer;
  }

  .slider::-moz-range-thumb {
    -webkit-appearance: none;
    background: white;
    height: 2em;
    width: 0.2em;
    border-radius: 5px;
    cursor: pointer;
  }

  .inputs {
    margin: 0 1em;
    display: flex;
    flex-direction: column;
  }

  label.group + label.group {
    margin-left: 1em;
  }

  .presets {
    display: grid;
    grid-gap: 0.5em;
    grid-template-columns: repeat(5, 5em);
  }

  .preset {
    height: 2em;
    border: 1px solid #d8d8d8;
    cursor: pointer;
  }

  .preview {
    height: 4em;
    width: 4em;
    border: 1px solid #d8d8d8;
    background: hsl(var(--hue), var(--saturation), var(--lightness));
  }
</style>

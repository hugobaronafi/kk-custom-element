<script lang="ts">
  import { fade } from "svelte/transition";
  import { Loader } from "@googlemaps/js-api-loader";

  import Invalid from "../../../shared/components/customElement/invalid.svelte";
  import CustomElement from "../../../shared/components/customElement/customElement.svelte";
  import { translate } from "../../../shared/stores/translate";
  import Loading from "../../../shared/components/loading.svelte";
  import sharedTranslations from "../../../shared/components/customElement/resources";
  import translations from "./_resources";

  interface IGoogleGeolocationConfig {
    apiKey: string;
    mapOptions: google.maps.MapOptions;
  }

  interface IGoogleGeolocationValue {
    coordinates: google.maps.LatLngLiteral;
  }

  let value: IGoogleGeolocationValue = { coordinates: undefined };
  let config: IGoogleGeolocationConfig;
  let disabled: boolean;

  let mapMount: HTMLDivElement;
  let map: google.maps.Map;
  let searchMount: HTMLInputElement;
  let search: google.maps.places.Autocomplete;
  let marker: google.maps.Marker;

  $: config && mapMount && loadMap();

  $: disabled && map && map.setOptions({ gestureHandling: "none" });

  const loadMap = async () => {
    await new Loader({
      apiKey: config.apiKey,
      libraries: ["places"],
    }).load();

    map = new google.maps.Map(mapMount, {
      zoom: 15,
      center: value.coordinates ?? {
        lat: 49.187841,
        lng: 16.604448,
      },
      streetViewControl: false,
      ...config.mapOptions,
    });

    if (!disabled) {
      map.addListener("click", onClick);
      map.addListener("center_changed", onCenterChanged);

      search = new google.maps.places.Autocomplete(searchMount, {
        origin: new google.maps.LatLng(value.coordinates),
      } as google.maps.places.AutocompleteOptions);

      search.addListener("place_changed", onPlaceChanged);
    }

    marker = new google.maps.Marker({
      position: map.getCenter(),
      clickable: false,
      map,
    });
  };

  const onClick = (event: google.maps.MouseEvent) => {
    map.panTo(event.latLng);
  };

  const onCenterChanged = () => {
    const center = map.getCenter();

    value.coordinates = center.toJSON();

    search.setOptions({
      origin: new google.maps.LatLng(value.coordinates),
    } as google.maps.places.AutocompleteOptions);

    marker.setPosition(center);
  };

  const onPlaceChanged = () => {
    const place = search.getPlace();

    if (place.geometry) {
      map.panTo(place.geometry.location);
      map.setZoom(config.mapOptions?.zoom ?? 15);
    }
  };

  const t = translate([translations, sharedTranslations]);
</script>

<CustomElement bind:value bind:config bind:disabled>
  <div transition:fade>
    {#if !disabled}
      <div class="group">
        <label class="group column filter"
          ><div class="label">
            {$t("search")}
          </div>
          <input
            class="input"
            type="text"
            placeholder={$t("placeholder")}
            bind:this={searchMount} />
        </label>
      </div>
    {/if}
    <div bind:this={mapMount} class="map" />
  </div>
  <div slot="loading">
    <Loading />
  </div>
  <div slot="invalid">
    <Invalid />
  </div>
</CustomElement>

<style>
  .filter {
    flex: 1;
  }

  .map {
    height: 20em;
  }
</style>

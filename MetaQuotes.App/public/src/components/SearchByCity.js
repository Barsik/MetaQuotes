import { geobaseService } from "./../services/geobaseService.js";
class SearchByCity extends HTMLElement {
  constructor() {
    super();

    this.city = "";
    this.success = false;
    this.loading = false;
    this.result = undefined;
    this.error = undefined;

    this.search = this.search.bind(this);
    this.cityText = this.cityText.bind(this);
    this.boundEvents = this.boundEvents.bind(this);
    this.onCityChange = this.onCityChange.bind(this);
  }

  cityText() {
    return this.querySelector("#city-text");
  }

  onCityChange(event) {
    this.city = event.target.value;
    this.render();
    const end = this.cityText().value.length;
    this.cityText().setSelectionRange(end, end);
    this.cityText().focus();
  }

  boundEvents() {
    this.querySelector("#city-search").onclick = this.search;
    this.cityText().oninput = this.onCityChange;
    this.cityText().onpressenter = this.search;
  }

  async search() {
    this.loading = true;
    this.success = true;
    this.result = undefined;
    this.error = undefined;
    this.render();
    const response = await geobaseService.searchByCity(this.city);
    this.success = response.success;
    if (response.success && response.result) {
      this.result = response.result;
    } else if (!!response.error) {
      this.error = response.error;
    }

    this.loading = false;
    this.render();
  }

  renderResult() {
    const items = (this.result ?? []).reduce(
      (prev, cur) =>
        prev +
        `<tr>
        <td>${cur.city}</td>
        <td>${cur.country}</td>
        <td>${cur.organization}</td>
        <td>${cur.postal}</td>
        <td>${cur.region}</td>
        <td>${cur.latitude}</td>
        <td>${cur.longitude}</td>
        </tr>`,
      ""
    );

    console.log(items);
    return !this.loading && !!this.success && !!this.result
      ? `<div class="search-by-city-result">
      <div class="city-result-wrapper">
      <table class="city-result">
      <thead>
      <tr>
      <td width="100px">City</td>
      <td width="100px">Country</td>
      <td width="200px">Organization</td>
      <td width="200px">Postal</td>
      <td width="100px">Region</td>
      <td width="90px">Latitude</td>
      <td width="90px">Longitude</td>
      </tr>
      </thead>
       ${items}
      </table>
      </div>
      </div>`
      : "";
  }

  renderNotFound() {
    return !this.loading && !!this.success && !this.result
      ? `<div class="not-found"><span>4😭4</span><span class="sub">ничего не найдено</span></div>`
      : ``;
  }

  async render() {
    const html = `<div class="search-by">
    <div class="search-wrapper">
      <div class="search-by-label">Поиск по городу</div>
      <span class="search">
        <input id="city-text" type="text" value="${this.city}" />
        <button id="city-search" ${
          this.city === "" ? "disabled" : ""
        }>🔍</button>
      </span>
    ${!!this.loading ? `<div class="loading">Загружаем...</div>` : ""}
    ${
      !this.success && !!this.error
        ? `<div class="error">${this.error}</div>`
        : ""
    }
    ${this.renderNotFound()}
    </div>
    ${this.renderResult()}
  </div>`;

    this.innerHTML = html;
    this.boundEvents();
  }

  connectedCallback() {
    this.render();
  }

  connectedCallback() {
    this.render();
  }
}

customElements.define("mq-search-by-city", SearchByCity);

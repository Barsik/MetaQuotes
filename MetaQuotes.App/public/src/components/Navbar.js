class Navbar extends HTMLElement {
  constructor() {
    super();
  }

  render() {
    this.innerHTML = `<div class="navbar">
    <ul>
    <li>
    <a href="#ip"><span>Гео-информации по IP</span></a>
    </li>
    <li>
    <a href="#city"><span>Поиск по городу</span></a>
    </li>
    </ul>
    </div>`;
  }

  connectedCallback() {
    this.render();
  }
}

customElements.define("mq-navbar", Navbar);

class Header extends HTMLElement {
  constructor() {
    super();
  }

  render() {
    this.innerHTML = `<div class="header"><span><img src="./../../images/logo.png"/></span></div>`;
  }

  connectedCallback() {
    this.render();
  }
}

customElements.define("mq-header", Header);

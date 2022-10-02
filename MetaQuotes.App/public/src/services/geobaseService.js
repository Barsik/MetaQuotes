const searchByIp = async (ip) => {
  try {
    const response = await fetch(`/ip/location?ip=${ip}`);
    if (response.status === 200) {
      return { result: await response.json(), success: true };
    } else if (response.status === 204 || response.status === 404) {
      return { result: undefined, success: true };
    } else {
      throw Error("Произошла ошибка");
    }
  } catch (e) {
    return { error: e.message, success: false };
  }
};
const searchByCity = async (city) => {
  try {
    const response = await fetch(`/city/locations?city=${city}`);
    if (response.status === 200) {
      return { result: await response.json(), success: true };
    } else if (response.status === 204 || response.status === 404) {
      return { result: undefined, success: true };
    } else {
      throw Error("Произошла ошибка");
    }
  } catch (e) {
    return { error: e.message, success: false };
  }
};

export const geobaseService = { searchByIp, searchByCity };

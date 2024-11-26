export default async function delay(time = 100) {
  return new Promise((resolve) => {
    setTimeout(resolve, time);
  })
}

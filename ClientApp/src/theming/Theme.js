// theme.js

// 1. import `extendTheme` function
import { extendTheme } from "@chakra-ui/react";

// 2. Add your color mode config
const config = {
  initialColorMode: "dark",
  useSystemColorMode: false,
};
const drawerTheme = {
  components: {
    Drawer: {
      variants: {
        permanent: {
          dialog: {
            pointerEvents: "auto",
          },
          dialogContainer: {
            pointerEvents: "none",
          },
        },
      },
    },
  },
};
// 3. extend the theme
const theme = extendTheme({ config, drawerTheme });

export default theme;

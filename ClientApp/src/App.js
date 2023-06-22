import { Box ,ChakraBaseProvider,Flex,Text} from '@chakra-ui/react';
import React, { Component } from 'react';
import { Route, Routes } from 'react-router-dom';
import Sidebar from './components/Sidebar';
import Toggle from './theming/Toggle';
import Chatbox from './components/Chatbox';


export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
      <Box>
        <Flex>
          <Sidebar/>
          <Box w='100%'>
            <Chatbox/>
          </Box>
        </Flex> 
      </Box>
    );
  }
}
